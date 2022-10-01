namespace Travellour.Business.DTOs.User;

public class PasswordChangeDto
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? NewPasswordAgain { get; set; }
}
