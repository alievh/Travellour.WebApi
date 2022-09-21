using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Forum : BaseEntity, IEntity
{
    public string? ForumTitle { get; set; }
    public string? ForumContent { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}
