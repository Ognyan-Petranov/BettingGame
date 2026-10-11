using FluentValidation;

namespace BettingGame.Domain.Entities.User
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.FirstName).SetValidator(new NameValidator());
            RuleFor(x => x.LastName).SetValidator(new NameValidator());
        }
    }
}