using PlayerWallet.Application.Interfaces.Providers;

namespace PlayerWallet.Infrastructure.Providers;
public class GameRulesProvider : IGameRulesProvider
{
	public decimal MinStake => 1m;

	public decimal MaxStake => 10m;

	public (double lose, double upTo2x, double twoToTenX) OutcomeDistribution
		=> (0.50, 0.40, 0.10);
}
