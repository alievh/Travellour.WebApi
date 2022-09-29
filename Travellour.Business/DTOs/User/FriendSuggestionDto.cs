using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.User;

public class FriendSuggestionDto
{
    public string? Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? UserName { get; set; }
    public Image? ProfileImage{ get; set; }
}
