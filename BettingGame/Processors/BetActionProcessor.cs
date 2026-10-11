using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Enums;
using BettingGame.Services.OutputWriter;

namespace BettingGame.Processors
{
    public class BetActionProcessor : IActionProcessor
    {
        private readonly string _actionType = ActionOperations.Bet;
        private readonly IConsoleOutputWriter _consoleOutputWriter;
        private readonly Random _random;

        public BetActionProcessor(IConsoleOutputWriter consoleOutputWriter, Random random)
        {
            _consoleOutputWriter = consoleOutputWriter;
            _random = random;
        }

        public string ActionType => _actionType;

        public void Process(User player, decimal amount)
        {
            if (player.Wallet.Balance < amount)
                throw new InvalidOperationException("Insufficient balance for the bet. Please deposit more funds.");

            int outcome = _random.Next(1, 101);

            BetPrizeEnums outcomeType = ResolvePrizeSize(outcome);
            decimal multiplier = ResolvePrizeMultiplier(outcomeType);
            decimal amountWon = CalculateAmountWon(multiplier, amount);

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

        private static decimal CalculateAmountWon(decimal multiplier, decimal amount)
        {
            return Math.Round(amount * multiplier, 2);
        }

        private static BetPrizeEnums ResolvePrizeSize(int outcome)
        {
            if (outcome > RewardThresholds.NoRewardThreshold && outcome <= RewardThresholds.FixedRewardThreshold)
            {
                return BetPrizeEnums.BasePrize;
            }
            else if (outcome > RewardThresholds.FixedRewardThreshold)
            {
                return BetPrizeEnums.BigPrize;
            }
            else
            {
                return BetPrizeEnums.NoPrize;
            }
        }

        private decimal ResolvePrizeMultiplier(BetPrizeEnums outcome)
        {
            return outcome switch
            {
                BetPrizeEnums.BasePrize => RewardMultipliers.MinimumRewardMultiplier + (decimal)_random.NextDouble() * RewardMultipliers.BaseRewardMultiplier,
                BetPrizeEnums.BigPrize => RewardMultipliers.MinimumRewardMultiplier + RewardMultipliers.BaseRewardMultiplier + ((decimal)_random.NextDouble() * RewardMultipliers.BigRewardMultiplier),
                _ => decimal.Zero,
            };
        }
    }
}
