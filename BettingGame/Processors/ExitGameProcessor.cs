
using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Exceptions;

namespace BettingGame.Processors
{
    public class ExitGameProcessor : IActionProcessor
    {
        public string ActionType => ActionOperations.Exit;

        public void Process(User player, decimal amount)
        {
            throw new GameOverException("Thank you for playing! Hope to see you again soon.");
        }
    }
}
