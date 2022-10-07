using Travellour.Business.DTOs.CommentDTO;
using Travellour.Core.Entities;

namespace Travellour.Business.Interfaces;

public interface ICommentService
{
    Task CreateCommentAsync(CommentCreateDto commentCreateDto);
    Task<List<CommentGetDto>> GetPostCommentsAsync(int id);
    Task<List<CommentGetDto>> GetForumCommentsAsync(int id);
    Task DeleteCommentAsync(int id);
}
