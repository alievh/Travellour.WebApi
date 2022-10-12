using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Group : BaseEntity, IEntity
{
    public string? GroupName { get; set; }
    public string? GroupDescription { get; set; }
    public string? GroupAdminId { get; set; }
    public AppUser? GroupAdmin { get; set; }
    public int? ProfileImageId { get; set; }
    public Image? ProfileImage { get; set; }
    public int? CoverImageId { get; set; }
    public Image? CoverImage { get; set; }
    public ICollection<AppUser>? GroupMembers { get; set; }
    public ICollection<Post>? GroupPosts { get; set; }
}
