using PlayerWallet.Application.Interfaces;
using PlayerWallet.Application.Interfaces.Providers;

namespace PlayerWallet.Application.Services;

public class GameService : IGameService
{
	private readonly IWalletService _wallet;
	private readonly IRandomProvider _randomProvider;
	private readonly IGameRulesProvider _rules;

	public GameService(IWalletService wallet, IRandomProvider rng, IGameRulesProvider rules)
	{
		_wallet = wallet;
		_randomProvider = rng;
		_rules = rules;
	}

	public (decimal win, decimal newBalance) Bet(int playerId, decimal stake, string requestId)
	{
		var st = Math.Round(stake, 2, MidpointRounding.AwayFromZero);
		if (st < _rules.MinStake || st > _rules.MaxStake)
			throw new ArgumentOutOfRangeException(nameof(stake), $"Stake must be between {_rules.MinStake} and {_rules.MaxStake}.");

		var balanceAfterStake = _wallet.Withdraw(playerId, st, requestId + ":stake");

		var r = _randomProvider.NextDouble();
		var (loseP, upTo2P, twoToTenP) = _rules.OutcomeDistribution;

		decimal win;
		if (r < loseP) win = 0m;
		else if (r < loseP + upTo2P) win = Round(RandomBetween(st * 0.01m, st * 2m));
		else win = Round(RandomBetween(st * 2m, st * 10m));

		var finalBalance = win > 0 ? _wallet.Deposit(playerId, win, requestId + ":win") : balanceAfterStake;
		return (win, finalBalance);
	}

	private decimal RandomBetween(decimal min, decimal max)
	{
		var u = (decimal)_randomProvider.NextDouble();
		return min + (max - min) * u;
	}

	private static decimal Round(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
}
