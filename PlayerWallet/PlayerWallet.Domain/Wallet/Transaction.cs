namespace PlayerWallet.Domain.Wallet;

public record Transaction(
	Guid TransactionId,
	int PlayerId,
	string RequestId,
	TransactionType Type,
	decimal Amount,    
	DateTime UtcTime
);