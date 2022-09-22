using AutoMapper;
using Travellour.Business.DTOs.Post;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PostGetDto> GetAsync(int id)
    {
        Post post = await _unitOfWork.PostRepository.GetAsync(n => n.Id == id, "User", "Likes", "Comments", "Images");
        if(post is null) throw new NullReferenceException();
        PostGetDto postDto = _mapper.Map<PostGetDto>(post);
        return postDto;
    }

    public async Task<List<PostGetDto>> GetAllAsync()
    {
        List<Post> posts = await _unitOfWork.PostRepository.GetAllAsync(n => !n.IsDeleted, "User", "Likes", "Comments", "Images");
        if (posts is null) throw new NullReferenceException();
        List<PostGetDto> postsDto = _mapper.Map<List<PostGetDto>>(posts);
        return postsDto;
    }

    public async Task CreateAsync(PostCreateDto postCreateDto)
    {
        Post post = _mapper.Map<Post>(postCreateDto);
        await _unitOfWork.PostRepository.CreateAsync(post);
    }
}
