using Travellour.Business.DTOs.ForumDTO;

namespace Travellour.Business.Interfaces;

public interface IForumService
{
    Task<ForumGetDto> GetAsync(int id);
    Task<List<ForumGetDto>> GetAllAsync();
    Task CreateAsync(ForumCreateDto forumCreateDto);
}
