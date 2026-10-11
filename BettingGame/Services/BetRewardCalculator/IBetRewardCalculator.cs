
using BettingGame.Constants;

namespace BettingGame.Services.BetRewardCalculator
{
    public interface IBetRewardCalculator
    {
        decimal CalculateReward(decimal betAmount);
    }
}
