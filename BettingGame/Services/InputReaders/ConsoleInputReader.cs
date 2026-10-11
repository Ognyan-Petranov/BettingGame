using BettingGame.Requests;
using BettingGame.Services.Helpers;
using BettingGame.Services.OutputWriter;
using FluentValidation;

namespace BettingGame.Services.InputReaders
{
    public class ConsoleInputReader : IConsoleInputReader
    {
        private readonly IConsoleOutputWriter _consoleOutputWriter;
        private readonly IValidator<ActionRequest> _actionRequestValidator;
        private readonly IActionRequestParser _actionRequestParser;

        public ConsoleInputReader(IConsoleOutputWriter consoleOutputWriter, IValidator<ActionRequest> actionRequestValidator, IActionRequestParser actionRequestParser)
        {
            _consoleOutputWriter = consoleOutputWriter;
            _actionRequestValidator = actionRequestValidator;
            _actionRequestParser = actionRequestParser;
        }

        public ActionRequest ReadInput()
        {
            while (true)
            {
                _consoleOutputWriter.SubmitActionMessage();
                var input = Console.ReadLine() ?? string.Empty;

                if (!_actionRequestParser.TryParse(input, out ActionRequest? request, out string? errorMessage))
                {
                    _consoleOutputWriter.CustomMessage(errorMessage);
                    continue;
                }

                var validationResult = _actionRequestValidator.Validate(request);

                if (validationResult.IsValid)
                {
                    return request;
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        _consoleOutputWriter.CustomMessage(error.ErrorMessage);
                    }
                }
            }
        }
    }
}
