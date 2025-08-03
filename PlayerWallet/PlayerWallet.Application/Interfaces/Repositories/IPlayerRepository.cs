using PlayerWallet.Domain.Wallet;

namespace PlayerWallet.Application.Interfaces.Repositories;

public interface IPlayerRepository
{
	Player? Get(int playerId);
	void Save(Player player);
}
