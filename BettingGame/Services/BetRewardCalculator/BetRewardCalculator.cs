
using BettingGame.Constants;
using BettingGame.Enums;

namespace BettingGame.Services.BetRewardCalculator
{
    public class BetRewardCalculator : IBetRewardCalculator
    {
        private readonly Random _random;

        public BetRewardCalculator(Random random)
        {
            _random = random;
        }

        public decimal CalculateReward(decimal betAmount)
        {
            int outcome = _random.Next(1, 101);

            BetPrizeEnums prize = ResolvePrizeSize(outcome);
            decimal multiplier = ResolvePrizeMultiplier(prize);

            return Math.Round(betAmount * multiplier, 2);
        }

        private static BetPrizeEnums ResolvePrizeSize(int outcome)
        {
            if (outcome > RewardThresholds.NoRewardUpperThreshold && outcome <= RewardThresholds.SmallRewardUpperThreshold)
            {
                return BetPrizeEnums.BasePrize;
            }
            else if (outcome > RewardThresholds.SmallRewardUpperThreshold)
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
