namespace PlayerWallet.Application.Interfaces;

public interface IGameService
{
	(decimal win, decimal newBalance) Bet(int playerId, decimal stake, string requestId);
}
