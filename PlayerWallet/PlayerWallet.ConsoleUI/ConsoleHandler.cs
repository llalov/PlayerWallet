using PlayerWallet.Application.Interfaces;

namespace PlayerWallet.ConsoleUI;
public sealed class ConsoleHandler
{
	private readonly IWalletService _walletService;
	private readonly IGameService _gameService;

	private static int NextPlayerId = 1;
	private readonly int _playerId;

	public ConsoleHandler(IWalletService walletService, IGameService gameService)
	{
		_walletService = walletService;
		_gameService = gameService;

		_playerId = NextPlayerId++;
	}

	public void Run()
	{
		PrintHelp();
		while (true)
		{
			Console.Write("> ");
			var line = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(line)) continue;

			var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
			var cmd = parts[0].ToLowerInvariant();

			try
			{
				switch (cmd)
				{
					case "help":
						PrintHelp();
						break;

					case "balance":
						Console.WriteLine($"Balance: ${_walletService.GetBalance(_playerId):0.00}");
						break;

					case "deposit":
						Require(parts, 2);
						var dep = ParseAmount(parts[1]);
						var depReq = NewRequestId();
						var depBal = _walletService.Deposit(_playerId, dep, depReq);
						Console.WriteLine($"Deposited ${dep:0.00}. New balance: ${depBal:0.00}");
						break;

					case "withdraw":
						Require(parts, 2);
						var w = ParseAmount(parts[1]);
						var wReq = NewRequestId();
						var wBal = _walletService.Withdraw(_playerId, w, wReq);
						Console.WriteLine($"Withdrew ${w:0.00}. New balance: ${wBal:0.00}");
						break;

					case "bet":
						Require(parts, 2);
						var stake = ParseAmount(parts[1]);
						var bReq = NewRequestId();
						var (win, newBal) = _gameService.Bet(_playerId, stake, bReq);
						Console.WriteLine(win > 0
							? $"You WON ${win:0.00}! New balance: ${newBal:0.00}"
							: $"You LOST. New balance: ${newBal:0.00}");
						break;

					case "exit":
					case "quit":
						Console.WriteLine("Goodbye!");
						return;

					default:
						Console.WriteLine("Unknown command. Type 'help' for options.");
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}

	private static void PrintHelp()
	{
		Console.WriteLine("Commands:");
		Console.WriteLine("  balance");
		Console.WriteLine("  deposit <amount>");
		Console.WriteLine("  withdraw <amount>");
		Console.WriteLine("  bet <stake between 1 and 10>");
		Console.WriteLine("  help");
		Console.WriteLine("  exit");
	}

	private static void Require(string[] parts, int n)
	{
		if (parts.Length < n) throw new ArgumentException("Missing argument.");
	}

	private static decimal ParseAmount(string s)
	{
		if (!decimal.TryParse(s, out var a) || a <= 0) throw new ArgumentException("Amount must be a positive number.");
		return Math.Round(a, 2, MidpointRounding.AwayFromZero);
	}

	private static string NewRequestId() => Guid.NewGuid().ToString("N");
}
