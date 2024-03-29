﻿using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.PostDTO;

public class PostGetDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public AppUser? User { get; set; }
    public Group? Group { get; set; }
    public int LikeCount { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public int CommentCount { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public List<string>? ImageUrls { get; set; }
    public string? FromCreateDate { get; set; }
}
