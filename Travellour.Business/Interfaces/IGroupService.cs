using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;

namespace Travellour.Business.Interfaces;

public interface IGroupService
{
    Task<GroupGetDto> GetAsync(int id);
    Task<List<GroupGetDto>> GetAllAsyn();
    Task<List<GroupGetDto>> SearchGroupByName(string groupName);
    Task<List<GroupGetDto>> GetMyGroupsAsync(string id);
    Task CreateAsync(GroupCreateDto groupCreateDto);
    Task<GroupProfileDto> GetGroupProfileAsync(int id);
    Task<List<PostGetDto>> GetAllGroupPostAsync(int id);
    Task ChangeGroupAsync(GroupUpdateDto groupUpdateDto);
    Task ChangeGroupPhotoAsync(int id, GroupPhotoDto groupPhotoDto);
    Task ChangeGroupCoverAsync(int id, GroupCoverDto groupCoverDto);
    Task JoinGroupAsync(int id);
    Task LeaveGroupAsync(int id);
    Task KickUserFromGroupAsync(string userId, int groupId);
}
