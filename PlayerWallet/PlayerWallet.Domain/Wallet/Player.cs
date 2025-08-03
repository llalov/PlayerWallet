namespace PlayerWallet.Domain.Wallet;

public sealed class Player
{
	public int Id { get; }
	public decimal Balance { get; private set; }

	private Player(int id, decimal balance)
	{
		Id = id;
		Balance = balance;
	}

	public static Player New(int id) => new(id, 0m);

	public void Apply(Transaction tx)
	{
		switch (tx.Type)
		{
			case TransactionType.Deposit:
			case TransactionType.Win:
				Balance += tx.Amount;
				break;

			case TransactionType.Withdraw:
			case TransactionType.Stake:
				var next = Balance - tx.Amount;
				if (next < 0) throw new InvalidOperationException("Insufficient funds.");
				Balance = next;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(tx.Type));
		}
	}
}
