using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.EventDTO;

public class EventGetDto
{
    public int Id { get; set; }
    public string? EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public string? EventCreatorId { get; set; }
    public List<string>? ImageUrls { get; set; }
    public ICollection<AppUser>? EventMembers { get; set; }
}
