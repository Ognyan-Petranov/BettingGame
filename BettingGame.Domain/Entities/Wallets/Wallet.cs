

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
            Balance += amount;
        }

        public void SubtractAmount(decimal amount)
        {
            Balance -= amount;
        }
    }
}
