using FluentValidation;

namespace BettingGame.Domain.Entities.User
{
    public class NameValidator : AbstractValidator<string>
    {
        public NameValidator()
        {
            RuleFor(name => name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Must(name => name.All(c => char.IsLetter(c)
                                    || char.IsWhiteSpace(c)
                                    || c is '\'' or '’' or '-' or '.'))
            .WithMessage("Name contains invalid characters.");
        }
    }
}