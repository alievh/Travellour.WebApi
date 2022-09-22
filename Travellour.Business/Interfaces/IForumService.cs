using Travellour.Business.DTOs.Forum;

namespace Travellour.Business.Interfaces;

public interface IForumService
{
    Task<ForumGetDto> GetAsync(int id);
    Task<List<ForumGetDto>> GetAllAsync();
}
