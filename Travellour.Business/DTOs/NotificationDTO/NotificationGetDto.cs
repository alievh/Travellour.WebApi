using Travellour.Core.Entities;
using Travellour.Core.Entities.Enum;

namespace Travellour.Business.DTOs.NotificationDTO;

public class NotificationGetDto
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public AppUser? Sender { get; set; }
    public NotificationStatus notificationStatus { get; set; }
    public Post? Post { get; set; }
}
