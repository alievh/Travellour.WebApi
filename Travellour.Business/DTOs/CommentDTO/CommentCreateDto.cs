namespace Travellour.Business.DTOs.CommentDTO;

public class CommentCreateDto
{
    public string? Content { get; set; }
    public int? PostId { get; set; }
    public int? ForumId { get; set; }
}
