using BettingGame.Constants;
using BettingGame.Requests;
using FluentValidation;

namespace BettingGame.Validators
{
    public class ActionRequestValidator : AbstractValidator<ActionRequest>
    {
        public ActionRequestValidator()
        {
            RuleFor(x => x.Operation)
                .NotEmpty()
                .WithMessage("Operation is required.")
                .Must(operation => ActionOperations.AllowedOperations.Contains(operation))
                .WithMessage("Invalid operation. Please enter 'deposit', 'withdraw', 'bet', or 'exit'."); ;

            RuleFor(x => x.Amount)
                .GreaterThan(decimal.Zero)
                .When(x => x.Operation != ActionOperations.Exit)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Amount)
            .InclusiveBetween(BetAmounts.MinimumBetAmount, BetAmounts.MaximumBetAmount)
            .When(x => x.Operation == ActionOperations.Bet)
            .WithMessage($"Please enter a valid bet amount between {BetAmounts.MinimumBetAmount} and {BetAmounts.MaximumBetAmount}.");
        }
    }
}
