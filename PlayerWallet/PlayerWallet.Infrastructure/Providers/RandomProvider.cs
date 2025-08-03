using PlayerWallet.Application.Interfaces.Providers;

namespace PlayerWallet.Infrastructure.Providers;
public sealed class RandomProvider : IRandomProvider
{
	private readonly Random _random = new();
	public double NextDouble() => _random.NextDouble();
}
