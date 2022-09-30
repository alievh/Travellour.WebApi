using Travellour.Business.DTOs.Group;

namespace Travellour.Business.Interfaces;

public interface IGroupService
{
    Task<GroupGetDto> GetAsync(int id);
    Task<List<GroupGetDto>> GetAllAsyn();
    Task CreateAsync(GroupCreateDto groupCreateDto);
}
