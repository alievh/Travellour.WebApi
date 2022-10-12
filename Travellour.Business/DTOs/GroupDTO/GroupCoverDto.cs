using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.GroupDTO;

public class GroupCoverDto
{
    public string? ImageName { get; set; }
    public IFormFile? ImageFile { get; set; }
}
