using FluentValidation;
using Travellour.Business.DTOs.AuthenticationDTO;

namespace Travellour.Business.Validator.Authentication
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(u => u.Firstname)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);
            RuleFor(u => u.Lastname)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);
            RuleFor(u => u.Password)
                .NotEmpty()
                .NotNull()
                .Equal(u => u.ConfirmPassword);
            RuleFor(u => u.Username)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(30);
        }
    }
}
