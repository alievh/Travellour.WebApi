using Travellour.Core.Entities.Base;
using Travellour.Core.Entities.Enum;

namespace Travellour.Core.Entities;

public class UserFriend : IEntity
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    public string? FriendId { get; set; }
    public AppUser? Friend { get; set; }
    public DateTime SenderDate { get; set; }
    public FriendRequestStatus Status { get; set; }
}
