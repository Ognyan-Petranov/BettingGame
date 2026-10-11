
using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Services.OutputWriter;

namespace BettingGame.Processors
{
    public class ExitGameProcessor : IActionProcessor
    {
        private readonly IConsoleOutputWriter _consoleOutputWriter;

        public ExitGameProcessor(IConsoleOutputWriter consoleOutputWriter)
        {
            _consoleOutputWriter = consoleOutputWriter;
        }

        public string ActionType => ActionOperations.Exit;

        public void Process(User player, decimal amount)
        {
            _consoleOutputWriter.PublishGoodbyeMessage();
        }
    }
}
