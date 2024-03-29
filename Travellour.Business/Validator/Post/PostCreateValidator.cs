﻿using FluentValidation;
using Travellour.Business.DTOs.PostDTO;

namespace Travellour.Business.Validator.Post
{
    public class PostCreateValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateValidator()
        {
            When(x => x.ImageFiles == null, () =>
            {
                RuleFor(p => p.Content).NotEmpty().NotNull();
            });
            When(x => x.ImageFiles != null, () =>
            {
                RuleForEach(x => x.ImageFiles).NotNull().Must(x => x.ContentType.Equals("image/jpeg") || x.ContentType.Equals("image/jpg") || x.ContentType.Equals("image/png")).WithMessage("File type is not image");
            });
        }
    }
}
