﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Business.Helpers;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;


public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _hostEnvironment;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _hostEnvironment = hostEnvironment;
        _userManager = userManager;
    }

    public async Task<UserGetDto> GetAsync(string id)
    {
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == id, "ProfileImage", "CoverImage");

        if (appUser is null)
        {
            throw new NullReferenceException();
        }

        UserGetDto userDto = _mapper.Map<UserGetDto>(appUser);
        userDto.ProfileImage = appUser.ProfileImage is not null ? appUser.ProfileImage.ImageUrl : "";
        userDto.CoverImage = appUser.CoverImage is not null ? appUser.CoverImage.ImageUrl : "";
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

        await _userManager.UpdateAsync(appUser);
    }

    public async Task<List<FriendSuggestionDto>> GetFriendSuggestionAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<AppUser> user = await _unitOfWork.UserRepository.GetAllAsync(predicate: u => u.Id != userId, includes: "ProfileImage");
        if (user is null) throw new NullReferenceException();
        List<FriendSuggestionDto> friendSuggestionDtos = _mapper.Map<List<FriendSuggestionDto>>(user);
        for (int i = 0; i < user.Count; i++)
        {
            friendSuggestionDtos[i].ImageUrl = user[i].ProfileImage?.ImageUrl;
        }

        return friendSuggestionDtos;
    }

    public async Task ChangeProfilePhotoAsync(ProfilePhotoDto profilePhotoDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId, "ProfileImage");
        if (appUser is null) throw new NullReferenceException();
#pragma warning disable CS8604 // Possible null reference argument.
        var image = new Image
        {
            ImageUrl = await profilePhotoDto.ImageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
        };
#pragma warning restore CS8604 // Possible null reference argument.
        await _unitOfWork.ImageRepository.CreateAsync(image);
        appUser.ProfileImage = image;
        appUser.ProfileImageId = image.Id;
        await _unitOfWork.SaveAsync();
    }

    public async Task ChangeCoverPhotoAsync(CoverPhotoDto coverPhotoDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId, "ProfileImage", "CoverImage");
        if (appUser is null) throw new NullReferenceException();
#pragma warning disable CS8604 // Possible null reference argument.
        var image = new Image
        {
            ImageUrl = await coverPhotoDto.ImageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
        };
#pragma warning restore CS8604 // Possible null reference argument.
        await _unitOfWork.ImageRepository.CreateAsync(image);
        appUser.CoverImage = image;
        appUser.CoverImageId = image.Id;
        await _unitOfWork.SaveAsync();
    }

    public async Task ChangeUserPasswordAsync(PasswordChangeDto passwordChangeDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId, "ProfileImage", "CoverImage");
        if (!await _userManager.CheckPasswordAsync(user, passwordChangeDto.OldPassword)) throw new NullReferenceException();
        if (passwordChangeDto.NewPassword?.Trim() != passwordChangeDto.NewPasswordAgain?.Trim())
        {
            throw new NullReferenceException();
        }
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, passwordChangeDto.NewPassword);
    }

    public async Task<UserProfileDto> GetUserProfileAsync(string? id)
    {
        AppUser user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == id, "ProfileImage", "CoverImage", "Posts");
        UserProfileDto userProfileDto = _mapper.Map<UserProfileDto>(user);
        userProfileDto.ProfileImage = user.ProfileImage is not null ? user.ProfileImage.ImageUrl : "";
        userProfileDto.CoverImage = user.CoverImage is not null ? user.CoverImage.ImageUrl : "";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        userProfileDto.PostCount = user.Posts is null ? 0 : user.Posts.Count;
        userProfileDto.FriendCount = user.Friends is null ? 0 : user.Friends.Count;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        return userProfileDto;
    }
}
