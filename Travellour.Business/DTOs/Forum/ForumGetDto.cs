using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.Forum;

public class ForumGetDto
{
    public int Id { get; set; }
    public string? ForumTitle { get; set; }
    public string? ForumContent { get; set; }
    public int CommentCount { get; set; }
}
