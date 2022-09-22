using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.Event;

public class EventGetDto
{
    public int Id { get; set; }
    public string? EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public AppUser? EventCreator { get; set; }
    public ICollection<AppUser>? EventMembers { get; set; }
}
