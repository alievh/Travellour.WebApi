using FluentValidation;
using Travellour.Business.DTOs.GroupDTO;

namespace Travellour.Business.Validator.Group
{
    public class GroupCreateValidator : AbstractValidator<GroupCreateDto>
    {
        public GroupCreateValidator()
        {
            RuleFor(g => g.GroupName).NotEmpty().NotNull();
            RuleFor(g => g.GroupDescription).NotEmpty().NotNull();
        }
    }
}
