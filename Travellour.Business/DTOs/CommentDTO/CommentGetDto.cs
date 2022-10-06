using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.CommentDTO;

public class CommentGetDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public AppUser? User { get; set; }
}
