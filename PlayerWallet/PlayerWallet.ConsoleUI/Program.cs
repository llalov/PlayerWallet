using PlayerWallet.Application.Interfaces;
using PlayerWallet.Application.Interfaces.Providers;
using PlayerWallet.Application.Interfaces.Repositories;
using PlayerWallet.Application.Services;
using PlayerWallet.ConsoleUI;
using PlayerWallet.Infrastructure.Providers;
using PlayerWallet.Infrastructure.Repositories;

public static class Program
{
	public static void Main(string[] args)
	{
		IPlayerRepository playerRepo = new InMemoryPlayerRepo();
		ITransactionRepository transactionRepo = new InMemoryTransactionRepo();
		IRandomProvider rng = new RandomProvider();
		IGameRulesProvider rules = new GameRulesProvider();

		IWalletService wallet = new WalletService(playerRepo, transactionRepo);
		IGameService game = new GameService(wallet, rng, rules);

		var handler = new ConsoleHandler(wallet, game);
		handler.Run();
	}
}
