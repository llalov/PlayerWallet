using PlayerWallet.Application.Interfaces.Repositories;
using PlayerWallet.Domain.Wallet;

namespace PlayerWallet.Infrastructure.Repositories;
public class InMemoryPlayerRepo : IPlayerRepository
{
	private readonly Dictionary<int, Player> _players = new();

	public Player? Get(int playerId) => _players.TryGetValue(playerId, out var p) ? p : null;

	public void Save(Player player) => _players[player.Id] = player;
}
