using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.Exceptions;
using Travellour.Business.Helpers;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _hostEnvironment;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<PostGetDto> GetAsync(int id)
    {
        Post post = await _unitOfWork.PostRepository.GetAsync(n => n.Id == id, "User.ProfileImage", "Likes", "Comments", "Images");
        if (post is null) throw new NotFoundException("Post Not Found!");
        PostGetDto postDto = _mapper.Map<PostGetDto>(post);
        postDto.Comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.CreateDate, n => n.PostId == id, "User.ProfileImage");
        if (post.Images != null)
        {
            List<string> imageUrls = new();
            foreach (var image in post.Images)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                imageUrls.Add(image.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            postDto.ImageUrls = imageUrls;
        }
        return postDto;
    }

    public async Task<List<PostGetDto>> GetAllAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<UserFriend> userFriends = await _unitOfWork.FriendRepository.GetAllAsync(n=> n.Id, n => (n.UserId == userId && n.Status == Core.Entities.Enum.FriendRequestStatus.Accepted) || (n.FriendId == userId && n.Status == Core.Entities.Enum.FriendRequestStatus.Accepted));
        var userIds = userFriends.Select(x => x.UserId);
        var friendIds = userFriends.Select(x => x.FriendId);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
        var posts = await _unitOfWork.PostRepository.GetAllAsync(n => n.CreateDate, n => n.GroupId == null && (friendIds.Contains(n.UserId) || userIds.Contains(n.UserId)), "User.ProfileImage", "Likes", "Comments", "Images");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        AppUser user = await _unitOfWork.UserRepository.GetAsync(n => n.Id == userId, "JoinedGroups");
        if(user.JoinedGroups != null)
        {
            foreach (var group in user.JoinedGroups)
            {
                List<Post> groupPosts = await _unitOfWork.PostRepository.GetAllAsync(n => n.CreateDate, n => n.GroupId == group.Id, "User.ProfileImage", "Likes", "Comments", "Images");
                posts.AddRange(groupPosts);
            }
        }

        List<PostGetDto> postsDto = _mapper.Map<List<PostGetDto>>(posts);
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
                postsDto[i].ImageUrls = imageUrls;
            }
            postsDto[i].FromCreateDate = posts[i].CreateDate.GetTimeBetween();
            postsDto[i].Comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.CreateDate, n => n.PostId == posts[i].Id, "User.ProfileImage");
        }
        return postsDto;
    }

    public async Task CreateAsync(PostCreateDto postCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Post post = _mapper.Map<Post>(postCreateDto);
        post.CreateDate = DateTime.UtcNow.AddHours(4);
        post.UserId = userId;
        post.User = appUser;
        if (postCreateDto.GroupId is not null)
        {
            post.GroupId = postCreateDto.GroupId;
        }
        if (postCreateDto.ImageFiles != null)
        {
            List<Image> images = new();
            foreach (var imageFile in postCreateDto.ImageFiles)
            {
                Image image = new()
                {
                    ImageUrl = await imageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
                };
                await _unitOfWork.ImageRepository.CreateAsync(image);
                images.Add(image);
            }
            post.Images = images;
        }
        await _unitOfWork.PostRepository.CreateAsync(post);
    }

    public async Task DeleteAsync(int id)
    {
        Post post = await _unitOfWork.PostRepository.GetAsync(n => n.Id == id);
        if (post is null) throw new NotFoundException("Post Not Found!");
        List<Image> images = await _unitOfWork.ImageRepository.GetAllAsync(n => n.Id, n => n.PostId == id);
        List<Notification> notifications = await _unitOfWork.NotificationRepository.GetAllAsync(n => n.CreateDate, n => n.Post == post);
        List<Comment> comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.CreateDate, n => n.PostId == id);
        foreach (var comment in comments)
        {
            await _unitOfWork.CommentRepository.DeleteAsync(comment);
        }
        foreach (var notification in notifications)
        {
            await _unitOfWork.NotificationRepository.DeleteAsync(notification);
        }
        foreach (var image in images)
        {
            await _unitOfWork.ImageRepository.DeleteAsync(image);
        }
        if (post == null) throw new NullReferenceException();
        await _unitOfWork.PostRepository.DeleteAsync(post);
    }

    public async Task<List<PostGetDto>> GetPostByUserIdAsync(string? id)
    {
        List<Post> posts = await _unitOfWork.PostRepository.GetAllAsync(n => n.CreateDate, n => n.UserId == id, "Images", "Likes", "User.ProfileImage", "Group");
        if (posts is null) throw new NotFoundException("Post Not Found!");
        List<PostGetDto> postDtos = _mapper.Map<List<PostGetDto>>(posts);
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
                postDtos[i].ImageUrls = imageUrls;
            }
            postDtos[i].Comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.CreateDate, n => n.PostId == posts[i].Id, "User.ProfileImage");
            postDtos[i].FromCreateDate = posts[i].CreateDate.GetTimeBetween();

        }
        return postDtos;
    }
}
