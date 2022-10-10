using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;
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
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == id && !n.IsDeleted, "GroupAdmin", "GroupMembers", "Image");
        if (group is null) throw new NullReferenceException();
        GroupGetDto groupGetDto = _mapper.Map<GroupGetDto>(group);
        if (group.Image is not null)
        {
            groupGetDto.GroupImage = group.Image?.ImageUrl;
        }
        return groupGetDto;
    }

    public async Task<List<GroupGetDto>> GetAllAsyn()
    {
        List<Group> groups = await _unitOfWork.GroupRepository.GetAllAsync(n => n.CreateDate, n=> !n.IsDeleted,"Image");
        if (groups is null) throw new NullReferenceException();
        List<GroupGetDto> groupGetDtos = _mapper.Map<List<GroupGetDto>>(groups);
        for (int i = 0; i < groups.Count; i++)
        {
            groupGetDtos[i].GroupImage = groups[i].Image?.ImageUrl;
        }
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
        if (groupCreateDto.ImageFile != null)
        {
            Image image = new()
            {
                ImageUrl = await groupCreateDto.ImageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
            };
            await _unitOfWork.ImageRepository.CreateAsync(image);
            group.ImageId = image.Id;
            group.Image = image;
        };
        await _unitOfWork.GroupRepository.CreateAsync(group);
    }

    public async Task<GroupProfileDto> GetGroupProfileAsync(int id)
    {
        Group group = await _unitOfWork.GroupRepository.GetAsync(n => n.Id == id && !n.IsDeleted, "GroupAdmin.ProfileImage", "GroupMembers", "Image", "GroupPosts");
        if (group is null) throw new NullReferenceException();
        GroupProfileDto groupProfileDto = _mapper.Map<GroupProfileDto>(group);
        if (group.Image is not null)
        {
            groupProfileDto.GroupImage = group.Image?.ImageUrl;
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        groupProfileDto.MemberCount = group.GroupMembers.Count;
        groupProfileDto.PostCount = group.GroupPosts.Count;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        return groupProfileDto;
    }

    public async Task<List<PostGetDto>> GetAllGroupPostAsync(int id)
    {
        List<Post> posts = await _unitOfWork.PostRepository.GetAllAsync(n => n.CreateDate, n => n.GroupId == id, "Group", "User.ProfileImage", "Images", "Likes", "Comments");
        if (posts is null) throw new NullReferenceException();
        List<PostGetDto> postGetDtos = _mapper.Map<List<PostGetDto>>(posts);
        for (int i = 0; i < posts.Count; i++)
        {
            List<Comment> comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.CreateDate, n => n.PostId == posts[i].Id, "User.ProfileImage");
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                postGetDtos[i].LikeCount = posts[i].Likes.Count;
                postGetDtos[i].CommentCount = posts[i].Comments.Count;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            postGetDtos[i].Comments = comments;
        }
        return postGetDtos;
    }

    public async Task JoinGroupAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Group group = await _unitOfWork.GroupRepository.GetAsync(u => u.Id == id, "GroupMembers");
        if (group == null) throw new NullReferenceException();
        group.GroupMembers?.Add(appUser);
        await _unitOfWork.GroupRepository.UpdateAsync(group);
    }
}
