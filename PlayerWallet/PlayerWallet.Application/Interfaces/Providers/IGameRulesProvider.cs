namespace PlayerWallet.Application.Interfaces.Providers;

public interface IGameRulesProvider
{
	decimal MinStake { get; }

	decimal MaxStake { get; }

	(double lose, double upTo2x, double twoToTenX) OutcomeDistribution { get; }
}
