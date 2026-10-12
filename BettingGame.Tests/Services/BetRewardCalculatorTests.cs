
using BettingGame.Constants;
using BettingGame.Services.BetRewardCalculator;
using BettingGame.Tests.Models;

namespace BettingGame.Tests.Services
{
    public class BetRewardCalculatorTests
    {
        private const double EXPECTED_RANDOM_OUTPUT = 0.5;

        [Fact]
        public void CalculateReward_WhenOutcomeIsAtNoRewardThreshold_ReturnsZero()
        {
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.NoRewardUpperThreshold));

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(decimal.Zero, result);
        }

        [Fact]
        public void CalculateReward_WhenOutcomeIsAtSmallRewardThreshold_ReturnsBasePrize()
        {
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.SmallRewardUpperThreshold, EXPECTED_RANDOM_OUTPUT));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + (decimal)EXPECTED_RANDOM_OUTPUT * RewardMultipliers.BaseRewardMultiplier;
            decimal expectedAmount = Math.Round(BetAmounts.MaximumBetAmount * expectedMultiplier, 2);

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(expectedAmount, result);
        }

        [Fact]
        public void CalculateReward_WhenOutcomeIsAtMaximumRewardThreshold_ReturnsMaximumPrize()
        {
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.BigRewardThreshold, EXPECTED_RANDOM_OUTPUT));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + RewardMultipliers.BaseRewardMultiplier + (decimal)EXPECTED_RANDOM_OUTPUT * RewardMultipliers.BigRewardMultiplier;
            decimal expectedAmount = Math.Round(BetAmounts.MaximumBetAmount * expectedMultiplier, 2);

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(expectedAmount, result);
        }

        [Fact]
        public void CalculateReward_WhenNoRewardThresholdIsPassed_ReturnsSmallReward()
        {
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.NoRewardUpperThreshold + 1, EXPECTED_RANDOM_OUTPUT));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + (decimal)EXPECTED_RANDOM_OUTPUT * RewardMultipliers.BaseRewardMultiplier;
            decimal expectedAmount = Math.Round(BetAmounts.MaximumBetAmount * expectedMultiplier, 2);

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(expectedAmount, result);
        }

        [Fact]
        public void CalculateReward_WhenSmallRewardThresholdIsPassed_ReturnsBigReward()
        {
            double expectedRandomOutput = 0.5;
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.SmallRewardUpperThreshold + 1, expectedRandomOutput));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + RewardMultipliers.BaseRewardMultiplier + (decimal)expectedRandomOutput * RewardMultipliers.BigRewardMultiplier;
            decimal expectedAmount = Math.Round(BetAmounts.MaximumBetAmount * expectedMultiplier, 2);

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(expectedAmount, result);
        }
    }
}
