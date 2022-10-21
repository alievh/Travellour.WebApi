using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.Exceptions;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddLikeAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Post post = await _unitOfWork.PostRepository.GetAsync(p => p.Id == id);
        if (post == null) throw new NotFoundException("Post Not Found!");

        Like like = new()
        {
            UserId = userId,
            User = appUser,
            PostId = id,
            Post = post
        };

        await _unitOfWork.LikeRepository.CreateAsync(like);
    }

    public async Task DeleteLikeAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Like like = await _unitOfWork.LikeRepository.GetAsync(l => l.UserId == userId && l.PostId == id);
        if (like == null) throw new NotFoundException("Like Not Found!");
        await _unitOfWork.LikeRepository.DeleteAsync(like);
    }
}
