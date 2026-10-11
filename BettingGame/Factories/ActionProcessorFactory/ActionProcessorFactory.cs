
using BettingGame.Processors;

namespace BettingGame.Factories.ActionProcessorFactory
{
    public class ActionProcessorFactory : IActionProcessorFactory
    {
        private readonly Dictionary<string, IActionProcessor> _actionProcessors;

        public ActionProcessorFactory(IEnumerable<IActionProcessor> actionProcessors)
        {
            _actionProcessors = actionProcessors.ToDictionary(ap => ap.ActionType);
        }

        public IActionProcessor GetActionProcessor(string action)
        {
            if (_actionProcessors.TryGetValue(action, out IActionProcessor? processor))
                return processor;

            throw new InvalidOperationException("Action processor not found.");
        }
    }
}
