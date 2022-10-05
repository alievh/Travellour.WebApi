using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Post : BaseEntity, IEntity
{
    public string? Content { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public int? GroupId { get; set; }
    public Group? Group { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Image>? Images { get; set; }
}
