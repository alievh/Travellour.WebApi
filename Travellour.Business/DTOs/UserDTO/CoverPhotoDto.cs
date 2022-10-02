using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.UserDTO;

public class CoverPhotoDto
{
    public string? ImageName { get; set; }
    public IFormFile? ImageFile { get; set; }
}
