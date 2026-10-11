
using BettingGame.Constants;
using BettingGame.Requests;
using BettingGame.Services.OutputWriter;
using BettingGame.Validators;

namespace BettingGame.Services.InputReaders
{
    public class ConsoleInputReader : IConsoleInputReader
    {
        private readonly IConsoleOutputWriter _consoleOutputWriter;

        public ConsoleInputReader(IConsoleOutputWriter consoleOutputWriter)
        {
            _consoleOutputWriter = consoleOutputWriter;
        }

        public ActionRequest ReadInput()
        {
            _consoleOutputWriter.SubmitActionMessage();
            var input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && string.Equals(input, ActionOperations.Exit, StringComparison.InvariantCultureIgnoreCase))
                return new ActionRequest() { Operation = ActionOperations.Exit, Amount = decimal.Zero };

            var splitInput = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitInput?.Length != 2)
            {
                _consoleOutputWriter.InvalidInputFormatMessage();
                return ReadInput();
            }

            string operation = splitInput?[0].ToLowerInvariant() ?? string.Empty;
            string amountString = splitInput?[1] ?? string.Empty;

            if (!decimal.TryParse(amountString, out var amount))
            {
                _consoleOutputWriter.InvalidTransactionAmountMessage();
                return ReadInput();
            }
            var request = new ActionRequest()
            {
                Operation = operation,
                Amount = amount
            };

            var validator = new ActionRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return ReadInput();
            }

            return request;
        }
    }
}
