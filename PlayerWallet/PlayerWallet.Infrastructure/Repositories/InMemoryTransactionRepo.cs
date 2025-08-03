using PlayerWallet.Application.Interfaces.Repositories;
using PlayerWallet.Domain.Wallet;

namespace PlayerWallet.Infrastructure.Repositories;

public class InMemoryTransactionRepo : ITransactionRepository
{
	private readonly Dictionary<int, List<Transaction>> _byPlayer = new();
	private readonly HashSet<(int PlayerId, string RequestId)> _processed = new();

	public void Add(Transaction transaction)
	{
		if (!_byPlayer.TryGetValue(transaction.PlayerId, out var list))
			_byPlayer[transaction.PlayerId] = list = new List<Transaction>();
		list.Add(transaction);
		_processed.Add((transaction.PlayerId, transaction.RequestId));
	}

	public void Remove(Guid transactionId)
	{
		foreach (var kv in _byPlayer)
		{
			var list = kv.Value;
			var idx = list.FindIndex(t => t.TransactionId == transactionId);
			if (idx >= 0)
			{
				var tx = list[idx];
				list.RemoveAt(idx);
				_processed.Remove((tx.PlayerId, tx.RequestId));
				break;
			}
		}
	}

	public bool ExistsByRequestId(int playerId, string requestId)
		=> _processed.Contains((playerId, requestId));

	public IReadOnlyList<Transaction> GetForPlayer(int playerId)
		=> _byPlayer.TryGetValue(playerId, out var list) ? list : Array.Empty<Transaction>();
}
