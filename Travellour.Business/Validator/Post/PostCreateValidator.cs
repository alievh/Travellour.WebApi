using FluentValidation;
using Travellour.Business.DTOs.PostDTO;

namespace Travellour.Business.Validator.Post
{
    public class PostCreateValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateValidator()
        {
            RuleFor(p => p.Content).NotEmpty().NotNull();
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            RuleFor(x => x.ImageFiles).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png")).WithMessage("File type is not image");
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
