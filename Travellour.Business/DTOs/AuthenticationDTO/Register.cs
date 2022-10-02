using Travellour.Core.Entities.Enum;

namespace Travellour.Business.DTOs.AuthenticationDTO;

public class Register
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public DateTime Birthday { get; set; }
    public Gender Gender { get; set; }
}
