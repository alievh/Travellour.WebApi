using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;
using Travellour.Core.Entities.Enum;

namespace Travellour.Business.Implementations;

public class FriendService : IFriendService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public FriendService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task FriendAcceptAsync(string? friendId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserFriend userFriend = await _unitOfWork.FriendRepository.GetAsync(n => n.FriendId == userId && n.UserId == friendId);
        userFriend.Status = FriendRequestStatus.Accepted;
        await _unitOfWork.FriendRepository.UpdateAsync(userFriend);
    }

    public async Task FriendAddAsync(string? friendId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new NullReferenceException();
        }
        AppUser userfriends = await _unitOfWork.UserRepository.GetAsync(u => u.Id == friendId);
        UserFriend userFriend = new()
        {
            UserId = userId,
            FriendId = userfriends?.Id,
            Status = FriendRequestStatus.Pending
        };
        await _unitOfWork.FriendRepository.CreateAsync(userFriend);
    }


    public async Task FriendRemoveAsync(string? friendId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserFriend userFriend = await _unitOfWork.FriendRepository.GetAsync(n => (n.UserId == userId && n.FriendId == friendId) || (n.UserId == friendId && n.FriendId == userId));
        if (userFriend is null)
        {
            throw new NullReferenceException();
        }
        await _unitOfWork.FriendRepository.DeleteAsync(userFriend);
    }

    public async Task FriendRejectAsync(string? friendId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserFriend userFriend = await _unitOfWork.FriendRepository.GetAsync(n => n.FriendId == userId && n.UserId == friendId);
        await _unitOfWork.FriendRepository.DeleteAsync(userFriend);
    }

    public async Task<List<UserGetDto>> GetFriendRequestAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<UserFriend> userFriends = await _unitOfWork.FriendRepository.GetAllAsync(n => n.Id, n => n.FriendId == userId && n.Status == FriendRequestStatus.Pending, "User.ProfileImage");
        if (userFriends is null) throw new NullReferenceException();
        List<UserGetDto> userGetDtos = new();
        foreach (var userFriend in userFriends)
        {
            UserGetDto userGetDto = _mapper.Map<UserGetDto>(userFriend.User);
            userGetDtos.Add(userGetDto);
        }
        return userGetDtos;
    }

    public async Task<List<FriendSuggestionDto>> GetFriendSuggestionAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<AppUser> users = await _unitOfWork.UserRepository.GetAllAsync(u => u.Id, u => u.Id != userId, "ProfileImage", "UserFriends");
        if (users is null) throw new NullReferenceException();
        List<AppUser> notFriends = new();
        foreach (var user in users)
        {
            UserFriend userFriend = await _unitOfWork.FriendRepository.GetAsync(u => (u.UserId == user.Id && u.FriendId == userId) || (u.FriendId == user.Id && u.UserId == userId));
            if (userFriend is null)
            {
                notFriends.Add(user);
            }
        }
        List<FriendSuggestionDto> friendSuggestionDtos = _mapper.Map<List<FriendSuggestionDto>>(notFriends);
        return friendSuggestionDtos;
    }

    public async Task<List<UserGetDto>> FriendGetAllAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<UserFriend> userFriends = await _unitOfWork.FriendRepository.GetAllAsync(n => n.Id, n => (n.FriendId == userId && n.Status == FriendRequestStatus.Accepted) || (n.UserId == userId && n.Status == FriendRequestStatus.Accepted), "User.ProfileImage", "Friend.ProfileImage");
        if (userFriends is null) throw new NullReferenceException();
        List<UserGetDto> userGetDtos = new();
        foreach (var userFriend in userFriends)
        {
            UserGetDto userGetDto = new();
            if (userFriend.UserId == userId)
            {
                userGetDto = _mapper.Map<UserGetDto>(userFriend.Friend);
            }else if (userFriend.FriendId == userId)
            {
                userGetDto = _mapper.Map<UserGetDto>(userFriend.User);
            }
            userGetDtos.Add(userGetDto);
        }
        return userGetDtos;
    }

    public async Task CancelFriendRequestAsync(string? friendId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        UserFriend userFriend = await _unitOfWork.FriendRepository.GetAsync(n => (n.UserId == userId && n.Status == FriendRequestStatus.Pending) || (n.FriendId == friendId && n.Status == FriendRequestStatus.Pending));
        if (userFriend is null)
        {
            throw new NullReferenceException();
        }
        await _unitOfWork.FriendRepository.DeleteAsync(userFriend);
    }

    public async Task<List<UserGetDto>> SearchFriendByUsernameAsync(string username)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
        List<UserFriend> userFriends = await _unitOfWork.FriendRepository.GetAllAsync(n => n.Id, n => (n.FriendId == userId && n.Status == FriendRequestStatus.Accepted && n.User.UserName.ToLower().StartsWith(username.Trim().ToLower())) || (n.UserId == userId && n.Status == FriendRequestStatus.Accepted && n.User.UserName.ToLower().StartsWith(username.Trim().ToLower())), "User.ProfileImage", "Friend.ProfileImage");
        #pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (userFriends is null) throw new NullReferenceException();
        List<UserGetDto> userGetDtos = new();
        foreach (var userFriend in userFriends)
        {
            UserGetDto userGetDto = new();
            if (userFriend.UserId == userId)
            {
                userGetDto = _mapper.Map<UserGetDto>(userFriend.Friend);
            }
            else if (userFriend.FriendId == userId)
            {
                userGetDto = _mapper.Map<UserGetDto>(userFriend.User);
            }
            userGetDtos.Add(userGetDto);
        }
        return userGetDtos;
    }
}
