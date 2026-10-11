
using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Services.OutputWriter;

namespace BettingGame.Processors
{
    public class DepositActionProcessor : IActionProcessor
    {
        private readonly string _actionType = ActionOperations.Deposit;
        private readonly IConsoleOutputWriter _consoleOutputWriter;

        public DepositActionProcessor(IConsoleOutputWriter consoleOutputWriter)
        {
            _consoleOutputWriter = consoleOutputWriter;
        }

        public string ActionType { get => _actionType; }

        public void Process(User player, decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Invalid deposit amount. Please enter a positive value.");
            }

            player.Wallet.AddAmount(amount);
            _consoleOutputWriter.DepositSuccessMessage(player.Wallet.Currency, amount, player.Wallet.Balance);
        }
    }
}
