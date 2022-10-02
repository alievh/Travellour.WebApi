using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.DTOs.ForumDTO;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class ForumService : IForumService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ForumService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ForumGetDto> GetAsync(int id)
    {
        Forum forum = await _unitOfWork.ForumRepository.GetAsync(n => n.Id == id && !n.IsDeleted, "User", "Likes", "Comments");
        if (forum is null) throw new NullReferenceException();
        ForumGetDto forumGetDto = _mapper.Map<ForumGetDto>(forum);
        return forumGetDto;
    }

    public async Task<List<ForumGetDto>> GetAllAsync()
    {
        List<Forum> forums = await _unitOfWork.ForumRepository.GetAllAsync(n => !n.IsDeleted, "User", "Likes", "Comments");
        if (forums is null) throw new NullReferenceException();
        List<ForumGetDto> forumGetDto = _mapper.Map<List<ForumGetDto>>(forums);
        for (int i = 0; i < forums.Count; i++)
        {
#       pragma warning disable CS8604 // Possible null reference argument.
            forumGetDto[i].CommentCount = forums[i].Comments.Count();
#pragma warning restore CS8604 // Possible null reference argument.
        }
        return forumGetDto;
    }

    public async Task CreateAsync(ForumCreateDto forumCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Forum forum = _mapper.Map<Forum>(forumCreateDto);
        forum.CreateDate = DateTime.UtcNow.AddHours(4);
        forum.UserId = userId;
        forum.User = appUser;
        await _unitOfWork.ForumRepository.CreateAsync(forum);
    }
}
