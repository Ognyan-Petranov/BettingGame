using BettingGame.Constants;
using BettingGame.Requests;

namespace BettingGame.Services.Helpers
{
    public class ActionRequestParser : IActionRequestParser
    {
        public bool TryParse(string input, out ActionRequest? request, out string? errorMessage)
        {
            request = null;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = "Invalid input format. Please enter operation followed by transaction amount.";
                return false;
            }

            if (string.Equals(input, ActionOperations.Exit, StringComparison.InvariantCultureIgnoreCase))
            {
                request = new ActionRequest() { Operation = ActionOperations.Exit, Amount = decimal.Zero };
                return true;
            }

            var splitInput = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitInput?.Length != 2)
            {
                errorMessage = "Invalid input format. Please enter operation followed by transaction amount.";
                return false;
            }

            string operation = splitInput?[0].ToLowerInvariant() ?? string.Empty;
            string amountString = splitInput?[1] ?? string.Empty;

            if (!decimal.TryParse(amountString, out var amount))
            {
                errorMessage = "Invalid transaction amount.";
                return false;
            }

            request = new ActionRequest()
            {
                Operation = operation,
                Amount = amount
            };

            return true;
        }
    }
}