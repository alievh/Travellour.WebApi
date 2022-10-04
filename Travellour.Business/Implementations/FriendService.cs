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
        UserFriend userFriend = await _unitOfWork.FriendRepository.GetAsync(n => n.UserId == userId && n.FriendId == friendId);
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
        List<UserFriend> userFriends = await _unitOfWork.FriendRepository.GetAllAsync(n => n.FriendId == userId && n.Status == FriendRequestStatus.Pending, "User.ProfileImage");
        if (userFriends is null) throw new NullReferenceException();
        List<UserGetDto> userGetDtos = new();
        foreach (var userFriend in userFriends)
        {
            UserGetDto userGetDto = _mapper.Map<UserGetDto>(userFriend.User);
            userGetDto.ProfileImage = userFriend.User?.ProfileImage?.ImageUrl;
            userGetDtos.Add(userGetDto);
        }
        return userGetDtos;
    }

    public async Task<List<FriendSuggestionDto>> GetFriendSuggestionAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<AppUser> user = await _unitOfWork.UserRepository.GetAllAsync(predicate: u => u.Id != userId, "ProfileImage", "UserFriends");
        if (user is null) throw new NullReferenceException();
        List<FriendSuggestionDto> friendSuggestionDtos = _mapper.Map<List<FriendSuggestionDto>>(user);
        for (int i = 0; i < user.Count; i++)
        {
            friendSuggestionDtos[i].ImageUrl = user[i].ProfileImage?.ImageUrl;
        }

        return friendSuggestionDtos;
    }

    public async Task<List<UserGetDto>> FriendGetAllAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<UserFriend> userFriends = await _unitOfWork.FriendRepository.GetAllAsync(n => n.FriendId == userId || n.UserId == userId && n.Status == FriendRequestStatus.Accepted, "User.ProfileImage", "Friend.ProfileImage");
        if (userFriends is null) throw new NullReferenceException();
        List<UserGetDto> userGetDtos = new();
        foreach (var userFriend in userFriends)
        {
            UserGetDto userGetDto = new();
            if (userFriend.UserId == userId)
            {
                userGetDto = _mapper.Map<UserGetDto>(userFriend.Friend);
                userGetDto.ProfileImage = userFriend.Friend?.ProfileImage?.ImageUrl;
            }else if (userFriend.FriendId == userId)
            {
                userGetDto = _mapper.Map<UserGetDto>(userFriend.User);
                userGetDto.ProfileImage = userFriend.User?.ProfileImage?.ImageUrl;

            }
            userGetDtos.Add(userGetDto);
        }
        return userGetDtos;
    }
}
