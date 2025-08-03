namespace PlayerWallet.Application.Interfaces;

public interface IWalletService
{
	decimal Deposit(int playerId, decimal amount, string requestId);
	decimal Withdraw(int playerId, decimal amount, string requestId);
	decimal GetBalance(int playerId);
}