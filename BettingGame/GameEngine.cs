using BettingGame.Constants;
using BettingGame.Domain.Entities.User;
using BettingGame.Factories.ActionProcessorFactory;
using BettingGame.Requests;
using BettingGame.Services.InputReaders;
using BettingGame.Services.OutputWriter;

namespace BettingGame
{
    public class GameEngine
    {
        private readonly IConsoleInputReader _consoleInputReader;
        private readonly IActionProcessorFactory _actionProcessorFactory;
        private readonly IConsoleOutputWriter _consoleOutputWriter;

        public GameEngine(IConsoleInputReader consoleInputReader, IActionProcessorFactory actionProcessorFactory, IConsoleOutputWriter consoleOutputWriter)
        {
            _consoleInputReader = consoleInputReader;
            _actionProcessorFactory = actionProcessorFactory;
            _consoleOutputWriter = consoleOutputWriter;
        }

        public void Run()
        {
            var firstName = "Jon";
            var lastName = "Doe";
            var email = "jon.doe@example.com";

            var player = new User(email, firstName, lastName);
            var validator = new UserValidator();
            var validationResult = validator.Validate(player);

            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException($"Player creation failed due to the following validation errors:{string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
            }

            while (true)
            {
                ActionRequest request = _consoleInputReader.ReadInput();

                if (request.Operation == ActionOperations.Exit)
                {
                    _consoleOutputWriter.PublishGoodbyeMessage();
                    break;
                }

                try
                {
                    var processor = _actionProcessorFactory.GetActionProcessor(request.Operation);
                    processor.Process(player, request.Amount);
                }
                catch (Exception ex)
                {
                    _consoleOutputWriter.CustomMessage(ex.Message);
                }
            }
        }
    }
}
