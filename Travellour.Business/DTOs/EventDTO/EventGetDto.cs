namespace Travellour.Business.DTOs.EventDTO;

public class EventGetDto
{
    public int Id { get; set; }
    public string? EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public List<string>? ImageUrls { get; set; }
}
