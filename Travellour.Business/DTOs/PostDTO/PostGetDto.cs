using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.PostDTO;

public class PostGetDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public AppUser? User { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public List<string>? ImageUrls { get; set; }
}
