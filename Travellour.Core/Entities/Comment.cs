using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Comment : BaseEntity, IEntity
{
    public string? Content { get; set; }
    public int? PostId { get; set; }
    public Post? Post { get; set; }
    public int? ForumId { get; set; }
    public Forum? Forum { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public int? CommentId { get; set; }
    public ICollection<Comment>? CommentReply { get; set; }
}
