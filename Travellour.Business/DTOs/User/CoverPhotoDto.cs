using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.User;

public class CoverPhotoDto
{
    public string? ImageName { get; set; }
    public IFormFile? ImageFile { get; set; }
}
