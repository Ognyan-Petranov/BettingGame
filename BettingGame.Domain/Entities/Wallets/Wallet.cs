

namespace BettingGame.Domain.Entities.Wallets
{
    public class Wallet
    {
        public Wallet()
        {
            Id = Guid.NewGuid();
            Balance = decimal.Zero;
            Currency = "$";
        }

        public Guid Id { get; private set; }

        public decimal Balance { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        public void AddAmount(decimal amount)   
        {
            if (amount <= 0)
                throw new InvalidOperationException("Deposit amount must be greater than zero.");
            Balance += amount;
        }

        public void SubtractAmount(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Withdrawal amount must be greater than zero.");
            if (Balance < amount)
                throw new InvalidOperationException($"Insufficient funds for withdrawal of {Currency}{amount}. Your current balance is: {Currency}{Balance}");

            Balance -= amount;
        }
    }
}
