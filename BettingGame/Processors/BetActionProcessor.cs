using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Services.BetRewardCalculator;
using BettingGame.Services.OutputWriter;

namespace BettingGame.Processors
{
    public class BetActionProcessor : IActionProcessor
    {
        private readonly string _actionType = ActionOperations.Bet;
        private readonly IConsoleOutputWriter _consoleOutputWriter;
        private readonly IBetRewardCalculator _betRewardCalculator;


        public BetActionProcessor(IConsoleOutputWriter consoleOutputWriter, IBetRewardCalculator betRewardCalculator)
        {
            _consoleOutputWriter = consoleOutputWriter;
            _betRewardCalculator = betRewardCalculator;
        }

        public string ActionType => _actionType;

        public void Process(User player, decimal amount)
        {
            if (player.Wallet.Balance < amount)
                throw new InvalidOperationException("Insufficient balance for the bet. Please deposit more funds.");

            decimal amountWon = _betRewardCalculator.CalculateReward(amount);

            if (amountWon == decimal.Zero)
            {
                player.Wallet.SubtractAmount(amount);
                _consoleOutputWriter.BetLostMessage(player.Wallet.Currency, player.Wallet.Balance);
            }
            else
            {
                decimal amountToAdd = amountWon - amount;
                player.Wallet.AddAmount(amountToAdd);

                _consoleOutputWriter.BetWonMessage(player.Wallet.Currency, amountWon, player.Wallet.Balance);
            }
        }
    }
}
