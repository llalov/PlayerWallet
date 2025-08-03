using PlayerWallet.Domain.Wallet;

namespace PlayerWallet.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
	void Add(Transaction transaction);
	void Remove(Guid transactionId);
	bool ExistsByRequestId(int playerId, string requestId);
	IReadOnlyList<Transaction> GetForPlayer(int playerId);
}
