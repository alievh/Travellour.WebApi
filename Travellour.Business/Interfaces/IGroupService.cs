using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;

namespace Travellour.Business.Interfaces;

public interface IGroupService
{
    Task<GroupGetDto> GetAsync(int id);
    Task<List<GroupGetDto>> GetAllAsyn();
    Task CreateAsync(GroupCreateDto groupCreateDto);
    Task<GroupProfileDto> GetGroupProfileAsync(int id);
    Task<List<PostGetDto>> GetAllGroupPostAsync(int id);
    Task JoinGroupAsync(int id);
}
