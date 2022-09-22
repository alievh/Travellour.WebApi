using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.Post;

public class PostCreateDto
{
    public string? Content { get; set; }
    public string? UserId { get; set; }
    public ICollection<Image>? Images { get; set; }
}
