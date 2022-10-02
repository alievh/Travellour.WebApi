using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.PostDTO;

public class PostCreateDto
{
    public string? Content { get; set; }
    public List<IFormFile>? ImageFiles { get; set; }
}
