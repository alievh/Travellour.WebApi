namespace Travellour.Business.DTOs.MessageDTO;

public class MessageDto
{
    public string? Content { get; set; }
    public string? SendUserId { get; set; }
    public DateTime SenderDate { get; set; } = DateTime.UtcNow.AddHours(4);
}
