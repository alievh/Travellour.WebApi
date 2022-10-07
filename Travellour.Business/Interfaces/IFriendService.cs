using Travellour.Business.DTOs.UserDTO;

namespace Travellour.Business.Interfaces;

public interface IFriendService
{
    Task FriendAddAsync(string? friendId);
    Task FriendAcceptAsync(string? friendId);
    Task FriendRemoveAsync(string? friendId);
    Task FriendRejectAsync(string? friendId);
    Task<List<UserGetDto>> GetFriendRequestAsync();
    Task<List<FriendSuggestionDto>> GetFriendSuggestionAsync();
    Task<List<UserGetDto>> FriendGetAllAsync();
    Task CancelFriendRequestAsync(string? friendId);
}
