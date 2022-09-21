using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Group : BaseEntity, IEntity
{
    public string? GroupName { get; set; }
    public string? GroupDescription { get; set; }
    public string? UserId { get; set; }
    public AppUser? GroupAdmin { get; set; }
    public ICollection<AppUser>? GroupMembers { get; set; }
    public int? ImageId { get; set; }
    public Image? Image { get; set; }
}
