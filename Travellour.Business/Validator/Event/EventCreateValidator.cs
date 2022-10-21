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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            RuleFor(x => x.ImageFiles).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png")).WithMessage("File type is not image");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
