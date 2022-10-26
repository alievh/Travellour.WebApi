namespace Travellour.Core.Entities;

public class Message
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime SenderDate { get; set; }
    public bool IsDeleted { get; set; }
    public string? SendUserId { get; set; }
    public AppUser? SendUser { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
}
