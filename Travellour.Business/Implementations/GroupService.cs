using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.Exceptions;
using Travellour.Business.Helpers;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class GroupService : IGroupService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _hostEnvironment;

    public GroupService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<GroupGetDto> GetAsync(int id)
    {
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == id && !n.IsDeleted, "GroupAdmin", "GroupMembers", "ProfileImage", "CoverImage");
        if (group is null) throw new NotFoundException("Group Not Found!");
        GroupGetDto groupGetDto = _mapper.Map<GroupGetDto>(group);
        return groupGetDto;
    }

    public async Task<List<GroupGetDto>> GetAllAsyn()
    {
        List<Group> groups = await _unitOfWork.GroupRepository.GetAllAsync(n => n.CreateDate, n => !n.IsDeleted, "GroupAdmin", "GroupMembers.ProfileImage", "ProfileImage", "CoverImage");
        if (groups is null) throw new NotFoundException("Group Not Found!");
        List<GroupGetDto> groupGetDtos = _mapper.Map<List<GroupGetDto>>(groups);
        return groupGetDtos;
    }

    public async Task<List<GroupGetDto>> GetMyGroupsAsync(string id)
    {
        List<Group> groups = await _unitOfWork.GroupRepository.GetAllAsync(n => n.CreateDate, n => n.GroupAdminId == id, "ProfileImage");
        if (groups is null) throw new NotFoundException("Group Not Found!");
        List<GroupGetDto> groupGetDtos = _mapper.Map<List<GroupGetDto>>(groups);
        return groupGetDtos;
    }

    public async Task CreateAsync(GroupCreateDto groupCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Group group = _mapper.Map<Group>(groupCreateDto);
        group.CreateDate = DateTime.UtcNow.AddHours(4);
        group.GroupAdminId = userId;
        group.GroupAdmin = appUser;
        await _unitOfWork.GroupRepository.CreateAsync(group);
    }

    public async Task ChangeGroupAsync(GroupUpdateDto groupUpdateDto)
    {
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == groupUpdateDto.Id);
        if (group is null) throw new NotFoundException("Group Not Found!");
        if (groupUpdateDto.GroupName?.Trim() == "" && groupUpdateDto.GroupDescription?.Trim() == "") throw new NullReferenceException();
        if(groupUpdateDto.GroupName != null && groupUpdateDto.GroupName.Trim() != "")
        {
            group.GroupName = groupUpdateDto.GroupName;
        }
        if(groupUpdateDto.GroupDescription != null && groupUpdateDto.GroupDescription.Trim() != "")
        {
            group.GroupDescription = groupUpdateDto.GroupDescription;
        }
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }

    public async Task<GroupProfileDto> GetGroupProfileAsync(int id)
    {
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == id && !n.IsDeleted, "GroupAdmin.ProfileImage", "GroupMembers", "ProfileImage", "CoverImage", "GroupPosts");
        if (group is null) throw new NotFoundException("Group Not Found!");
        GroupProfileDto groupProfileDto = _mapper.Map<GroupProfileDto>(group);
        return groupProfileDto;
    }

    public async Task<List<PostGetDto>> GetAllGroupPostAsync(int id)
    {
        List<Post> posts = await _unitOfWork.PostRepository.GetAllAsync(n => n.CreateDate, n => n.GroupId == id, "Group", "User.ProfileImage", "Images", "Likes", "Comments");
        if (posts is null) throw new NotFoundException("Posts Not Found!");
        List<PostGetDto> postGetDtos = _mapper.Map<List<PostGetDto>>(posts);
        for (int i = 0; i < posts.Count; i++)
        {
            if (posts[i].Images != null)
            {
                List<string> imageUrls = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var image in posts[i].Images)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    imageUrls.Add(image.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                postGetDtos[i].ImageUrls = imageUrls;
            }
            posts[i].Comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.CreateDate, n => n.PostId == posts[i].Id, "User.ProfileImage");
        }
        return postGetDtos;
    }

    public async Task JoinGroupAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Group group = await _unitOfWork.GroupRepository.GetAsync(u => u.Id == id, "GroupMembers");
        if (group == null) throw new NotFoundException("Group Not Found!");
        group.GroupMembers?.Add(appUser);
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }

    public async Task LeaveGroupAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Group group = await _unitOfWork.GroupRepository.GetAsync(u => u.Id == id, "GroupMembers");
        if (group == null) throw new NotFoundException("Group Not Found!");
        group.GroupMembers?.Remove(appUser);
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }

    public async Task KickUserFromGroupAsync(string userId, int groupId)
    {
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        if (appUser == null) throw new NotFoundException("User Not Found!");
        Group group = await _unitOfWork.GroupRepository.GetAsync(u => u.Id == groupId, "GroupMembers");
        if (group == null) throw new NotFoundException("Group Not Found!");
        group.GroupMembers?.Remove(appUser);
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }

    public async Task ChangeGroupPhotoAsync(int id, GroupPhotoDto groupPhotoDto)
    {
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == id, "ProfileImage");
        if (group == null) throw new NotFoundException("Group Not Found!");
#pragma warning disable CS8604 // Possible null reference argument.
        var image = new Image
        {
            ImageUrl = await groupPhotoDto.ImageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
        };
#pragma warning restore CS8604 // Possible null reference argument.
        await _unitOfWork.ImageRepository.CreateAsync(image);
        group.ProfileImage = image;
        group.ProfileImageId = image.Id;
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }

    public async Task ChangeGroupCoverAsync(int id, GroupCoverDto groupCoverDto)
    {
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == id, "CoverImage");
        if (group == null) throw new NotFoundException("Group Not Found!");
#pragma warning disable CS8604 // Possible null reference argument.
        var image = new Image
        {
            ImageUrl = await groupCoverDto.ImageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
        };
#pragma warning restore CS8604 // Possible null reference argument.
        await _unitOfWork.ImageRepository.CreateAsync(image);
        group.CoverImage = image;
        group.CoverImageId = image.Id;
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }

    public async Task<List<GroupGetDto>> SearchGroupByName(string groupName)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        List<Group> groups = await _unitOfWork.GroupRepository.GetAllAsync(n => n.CreateDate, n => n.GroupName.ToLower().Contains(groupName.Trim().ToLower()) , "GroupAdmin", "GroupMembers.ProfileImage", "ProfileImage", "CoverImage");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (groups is null) throw new NullReferenceException();
        List<GroupGetDto> groupGetDtos = _mapper.Map<List<GroupGetDto>>(groups);
        return groupGetDtos;
    }
}
