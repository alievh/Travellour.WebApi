using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.GroupDTO;

public class GroupPhotoDto
{
    public string? ImageName { get; set; }
    public IFormFile? ImageFile { get; set; }
}
