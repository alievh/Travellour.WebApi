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
        appUser.Firstname = userUpdateDto.Firstname is not null || userUpdateDto.Firstname?.Trim() != "" ? userUpdateDto.Firstname : appUser.Firstname;
        appUser.Lastname = userUpdateDto.Lastname is not null || userUpdateDto.Lastname?.Trim() != "" ? userUpdateDto.Lastname : appUser.Lastname;
        appUser.UserName = userUpdateDto.Username is not null || userUpdateDto.Username?.Trim() != "" ? userUpdateDto.Username : appUser.UserName;
        await _unitOfWork.UserRepository.UpdateAsync(appUser);
    }
}
