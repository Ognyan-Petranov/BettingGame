
using BettingGame.Processors;

namespace BettingGame.Factories.ActionProcessorFactory
{
    public interface IActionProcessorFactory
    {
        IActionProcessor GetActionProcessor(string action);
    }
}
