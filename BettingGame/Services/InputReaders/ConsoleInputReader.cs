
using BettingGame.Constants;
using BettingGame.Requests;
using BettingGame.Validators;

namespace BettingGame.Services.InputReaders
{
    public class ConsoleInputReader : IConsoleInputReader
    {
        public ActionRequest ReadInput()
        {
            Console.WriteLine("Please, submit action (or type 'exit' to quit):");
            var input = Console.ReadLine();

            var splitInput = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitInput?.Length == 1 && splitInput[0].ToLowerInvariant() == ActionOperations.Exit)
            {
                return new ActionRequest() { Operation = ActionOperations.Exit, Amount = decimal.Zero };
            }

            if (splitInput?.Length != 2)
            {
                Console.WriteLine("Invalid input format. Please enter operation followed by transaction amount.");
                return ReadInput();
            }

            var operation = splitInput?[0].ToLowerInvariant();
            var amountString = splitInput?[1];

            if (!decimal.TryParse(amountString, out var amount))
            {
                Console.WriteLine("Please enter a valid transaction amount.");
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
