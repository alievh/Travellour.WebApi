using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Like : BaseEntity, IEntity
{
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public int PostId { get; set; }
    public Post? Post { get; set; }
}
