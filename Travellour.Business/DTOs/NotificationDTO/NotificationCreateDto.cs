namespace Travellour.Business.DTOs.NotificationDTO;

public class NotificationCreateDto
{
    public string? Message { get; set; }
    public string? ReceiverId { get; set; }
    public int PostId { get; set; }
}
