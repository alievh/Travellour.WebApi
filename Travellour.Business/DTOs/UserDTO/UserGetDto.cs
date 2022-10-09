using Travellour.Core.Entities.Enum;

namespace Travellour.Business.DTOs.UserDTO;

public class UserGetDto
{
    public string? Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? UserName { get; set; }
    public string? ProfileImage { get; set; }
    public string? CoverImage { get; set; }
    public int NotificationCount { get; set; }
}
