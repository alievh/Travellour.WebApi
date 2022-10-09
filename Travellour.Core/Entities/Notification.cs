using Travellour.Core.Entities.Base;
using Travellour.Core.Entities.Enum;

namespace Travellour.Core.Entities;

public class Notification : BaseEntity, IEntity
{
    public string? Message { get; set; }
    public string? SenderId { get; set; }
    public AppUser? Sender { get; set; }
    public string? ReceiverId { get; set; }
    public AppUser? Receiver { get; set; }
    public Post? Post { get; set; }
    public NotificationStatus NotificationStatus { get; set; }
}
