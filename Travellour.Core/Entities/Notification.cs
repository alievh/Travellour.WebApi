using Travellour.Core.Entities.Base;
using Travellour.Core.Entities.Enum;

namespace Travellour.Core.Entities;

public class Notification : BaseEntity, IEntity
{
    public string? Message { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public NotificationStatus NotificationStatus { get; set; }
}
