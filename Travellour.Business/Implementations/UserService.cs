using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.DTOs.User;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;


public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserGetDto> GetAsync(string id)
    {
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == id, includes: "ProfileImage");

        if (appUser is null)
        {
            throw new NullReferenceException();
        }

        UserGetDto userDto = _mapper.Map<UserGetDto>(appUser);
        userDto.ProfileImage = appUser.ProfileImage is not null ? appUser.ProfileImage.ImageUrl : "";
        return userDto;
    }


    public async Task UpdateAsync(UserUpdateDto userUpdateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        if (appUser is null) throw new NullReferenceException();
        AppUser checkUsername = await _unitOfWork.UserRepository.GetAsync(u => u.UserName == userUpdateDto.Username);
        if (checkUsername is not null) throw new ArgumentException();

        if (userUpdateDto.Firstname is not null && userUpdateDto.Firstname?.Trim() != "")
        {
            appUser.Firstname = userUpdateDto.Firstname?.Trim();
        }

        if (userUpdateDto.Lastname is not null && userUpdateDto.Lastname?.Trim() != "")
        {
            appUser.Lastname = userUpdateDto.Lastname?.Trim();
        }

        if (userUpdateDto.Username is not null && userUpdateDto.Username?.Trim() != "")
        {
            appUser.UserName = userUpdateDto.Username?.Trim();
        }

        await _unitOfWork.UserRepository.UpdateAsync(appUser);
    }

    public async Task<List<FriendSuggestionDto>> GetFriendSuggestionAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<AppUser> appUsers = await _unitOfWork.UserRepository.GetAllAsync(predicate: u => u.Id != userId, includes: "ProfileImage");
        if (appUsers is null) throw new NullReferenceException();
        List<FriendSuggestionDto> friendSuggestionDtos = _mapper.Map<List<FriendSuggestionDto>>(appUsers);
        return friendSuggestionDtos;
    }
}
