using Travellour.Business.DTOs.User;

namespace Travellour.Business.Interfaces;

public interface IUserService
{
    Task<UserGetDto> GetAsync(string id);
    Task UpdateAsync(UserUpdateDto userUpdateDto);
    Task<List<FriendSuggestionDto>> GetFriendSuggestionAsync();
    Task ChangeProfilePhotoAsync(ProfilePhotoDto profilePhotoDto);
    Task ChangeCoverPhotoAsync(CoverPhotoDto coverPhotoDto);
}
