
using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Services.OutputWriter;

namespace BettingGame.Processors
{
    public class WithdrawActionProcessor : IActionProcessor
    {
        private readonly string _actionType = ActionOperations.Withdraw;
        private readonly IConsoleOutputWriter _consoleOutputWriter;

        public WithdrawActionProcessor(IConsoleOutputWriter consoleOutputWriter)
        {
            _consoleOutputWriter = consoleOutputWriter;
        }

        public string ActionType => _actionType;

        public void Process(User player, decimal amount)
        {
            if (player.Wallet.Balance < amount)
            {
                throw new InvalidOperationException($"Insufficient funds for withdrawal of {player.Wallet.Currency}{amount}. Your current balance is: {player.Wallet.Currency}{player.Wallet.Balance}");
            }
            else
            {
                player.Wallet.SubtractAmount(amount);
                _consoleOutputWriter.WithdrawSuccessMessage(player.Wallet.Currency, amount, player.Wallet.Balance);
            }
        }
    }
}
