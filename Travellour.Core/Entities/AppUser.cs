using Microsoft.AspNetCore.Identity;
using Travellour.Core.Entities.Base;
using Travellour.Core.Entities.Enum;

namespace Travellour.Core.Entities;

public class AppUser : IdentityUser, IEntity
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime RegisterDate { get; set; }
    public Gender Gender { get; set; }
    public int? ImageId { get; set; }
    public Image? ProfileImage { get; set; }
    public ICollection<AppUser>? Friends { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}
