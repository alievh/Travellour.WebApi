using Travellour.Business.DTOs.Group;

namespace Travellour.Business.Interfaces;

public interface IGroupService
{
    Task<List<GroupGetDto>> GetAllAsyn();
    Task CreateAsync(GroupCreateDto groupCreateDto);
}
