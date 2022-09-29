using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.Group;

public class GroupCreateDto
{
    public string? GroupName { get; set; }
    public string? GroupDescription { get; set; }
    public IFormFile? ImageFile { get; set; }
}
