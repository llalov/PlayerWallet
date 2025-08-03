using PlayerWallet.Application.Interfaces;
using PlayerWallet.Application.Interfaces.Repositories;
using PlayerWallet.Domain.Wallet;

namespace PlayerWallet.Application.Services;

public class WalletService : IWalletService
{
	private readonly IPlayerRepository _players;
	private readonly ITransactionRepository _transactions;
	private readonly Func<DateTime> _utcNow;
	private static readonly Dictionary<int, object> _locks = new();

	public WalletService(IPlayerRepository players, ITransactionRepository txs, Func<DateTime>? utcNow = null)
	{
		_players = players;
		_transactions = txs;
		_utcNow = utcNow ?? (() => DateTime.UtcNow);
	}

	public decimal Deposit(int playerId, decimal amount, string requestId)
		=> Apply(playerId, amount, requestId, TransactionType.Deposit);

	public decimal Withdraw(int playerId, decimal amount, string requestId)
		=> Apply(playerId, amount, requestId, TransactionType.Withdraw);

	public decimal GetBalance(int playerId)
		=> (_players.Get(playerId) ?? Player.New(playerId)).Balance;

	private decimal Apply(int playerId, decimal amount, string requestId, TransactionType type)
	{
		if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

		var locker = GetLock(playerId);
		lock (locker)
		{
			if (_transactions.ExistsByRequestId(playerId, requestId))
				return GetBalance(playerId); // idempotent retry

			var transaction = new Transaction(
				TransactionId: Guid.NewGuid(),
				PlayerId: playerId,
				RequestId: requestId,
				Type: type,
				Amount: Math.Round(amount, 2, MidpointRounding.AwayFromZero),
				UtcTime: _utcNow()
			);

			var player = _players.Get(playerId) ?? Player.New(playerId);

			try
			{
				_transactions.Add(transaction);
				player.Apply(transaction);
				_players.Save(player);
				return player.Balance;
			}
			catch
			{
				_transactions.Remove(transaction.TransactionId);
				throw;
			}
		}
	}

	private static object GetLock(int playerId)
	{
		lock (_locks)
		{
			if (!_locks.TryGetValue(playerId, out var obj))
			{
				obj = new object();
				_locks[playerId] = obj;
			}
			return obj;
		}
	}
}
