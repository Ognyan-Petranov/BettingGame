
using BettingGame.Domain.Entities.Wallets;

namespace BettingGame.Domain.Entities.User
{
    public class User
    {
        public User(string email, string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            CreatedOn = DateTime.UtcNow;
            LastUpdatedOn = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public string Email { get; private set; } = string.Empty;

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public Wallet Wallet { get; private set; } = new();

        public DateTime CreatedOn { get; private set; }

        public DateTime LastUpdatedOn { get; private set; }

        public DateTime? DeletedOn { get; private set; }
    }
}
