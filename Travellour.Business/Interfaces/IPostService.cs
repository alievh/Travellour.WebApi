using Travellour.Business.DTOs.PostDTO;

namespace Travellour.Business.Interfaces;

public interface IPostService
{
    Task<PostGetDto> GetAsync(int id);
    Task<List<PostGetDto>> GetAllAsync();
    Task CreateAsync(PostCreateDto postCreateDto);
    Task DeleteAsync(int id);
    Task<List<PostGetDto>> GetPostByUserIdAsync(string? id);
}
