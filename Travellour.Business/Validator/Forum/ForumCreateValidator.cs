using FluentValidation;
using Travellour.Business.DTOs.ForumDTO;

namespace Travellour.Business.Validator.Forum
{
    public class ForumCreateValidator : AbstractValidator<ForumCreateDto>
    {
        public ForumCreateValidator()
        {
            RuleFor(f => f.ForumTitle).NotEmpty().NotNull();
            RuleFor(f => f.ForumContent).NotEmpty().NotNull();
        }
    }
}
