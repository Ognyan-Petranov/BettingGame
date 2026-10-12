
using BettingGame.Constants;
using BettingGame.Services.BetRewardCalculator;
using BettingGame.Tests.Models;

namespace BettingGame.Tests.Services
{
    public class BetRewardCalculatorTests
    {
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
            double expectedRandomOutput = 0.5;
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.SmallRewardUpperThreshold, expectedRandomOutput));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + (decimal)expectedRandomOutput * RewardMultipliers.BaseRewardMultiplier;
            decimal expectedAmount = Math.Round(BetAmounts.MaximumBetAmount * expectedMultiplier, 2);

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(expectedAmount, result);
        }

        [Fact]
        public void CalculateReward_WhenOutcomeIsAtMaximumRewardThreshold_ReturnsMaximumPrize()
        {
            double expectedRandomOutput = 0.5;
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.BigRewardThreshold, expectedRandomOutput));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + RewardMultipliers.BaseRewardMultiplier + (decimal)expectedRandomOutput * RewardMultipliers.BigRewardMultiplier;
            decimal expectedAmount = Math.Round(BetAmounts.MaximumBetAmount * expectedMultiplier, 2);

            var result = betRewardCalculator.CalculateReward(BetAmounts.MaximumBetAmount);

            Assert.Equal(expectedAmount, result);
        }

        [Fact]
        public void CalculateReward_WhenNoRewardThresholdIsPassed_ReturnsSmallReward()
        {
            double expectedRandomOutput = 0.5;
            var betRewardCalculator = new BetRewardCalculator(new TestRandom(RewardThresholds.NoRewardUpperThreshold + 1, expectedRandomOutput));
            decimal expectedMultiplier = RewardMultipliers.MinimumRewardMultiplier + (decimal)expectedRandomOutput * RewardMultipliers.BaseRewardMultiplier;
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
