
using BettingGame.Domain.Entities.User;

namespace BettingGame.Processors
{
    public interface IActionProcessor
    {
        string ActionType { get; }

        void Process(User player, decimal amount);
    }
}
