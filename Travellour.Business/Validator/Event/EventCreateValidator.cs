using FluentValidation;
using Travellour.Business.DTOs.EventDTO;

namespace Travellour.Business.Validator.Event
{
    public class EventCreateValidator : AbstractValidator<EventCreateDto>
    {
        public EventCreateValidator()
        {
            RuleFor(e => e.EventTitle).NotEmpty().NotNull();
            RuleFor(e => e.EventDescription).NotEmpty().NotNull();
            When(x => x.ImageFiles != null, () =>
            {
                RuleForEach(x => x.ImageFiles).NotNull().Must(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png")).WithMessage("File type is not image");
            });
        }
    }
}
