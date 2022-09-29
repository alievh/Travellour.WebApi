using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.Post;
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
        if(post is null) throw new NullReferenceException();
        PostGetDto postDto = _mapper.Map<PostGetDto>(post);
        if(post.Images != null)
        {
            List<string> imageUrls = new();
            foreach(var image in post.Images)
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
        List<Post> posts = await _unitOfWork.PostRepository.GetAllAsync(n => !n.IsDeleted, "User.ProfileImage", "Likes", "Comments", "Images");
        
        if (posts is null) throw new NullReferenceException();
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
        if (post == null) throw new NullReferenceException();
        await _unitOfWork.PostRepository.DeleteAsync(post);
    }
}
