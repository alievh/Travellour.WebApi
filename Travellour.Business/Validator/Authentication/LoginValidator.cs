using FluentValidation;
using Travellour.Business.DTOs.AuthenticationDTO;

namespace Travellour.Business.Validator.Authentication
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(u => u.Username).NotEmpty().NotNull();
            RuleFor(u => u.Password).NotEmpty().NotNull();
        }
    }
}
