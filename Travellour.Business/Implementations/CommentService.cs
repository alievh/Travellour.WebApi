using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.DTOs.CommentDTO;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task CreateCommentAsync(CommentCreateDto commentCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Comment comment = _mapper.Map<Comment>(commentCreateDto);
        comment.UserId = userId;
        comment.User = appUser;
        await _unitOfWork.CommentRepository.CreateAsync(comment);
    }

    public async Task DeleteCommentAsync(int id)
    {
        Comment comment = await _unitOfWork.CommentRepository.GetAsync(u => u.Id == id);
        if (comment == null) throw new NullReferenceException();
        await _unitOfWork.CommentRepository.DeleteAsync(comment);
    }

    public async Task<List<CommentGetDto>> GetForumCommentsAsync(int id)
    {
        List<Comment> comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.ForumId == id, "User.ProfileImage");
        List<CommentGetDto> commentGetDtos = _mapper.Map<List<CommentGetDto>>(comments);
        return commentGetDtos;
    }

    public async Task<List<CommentGetDto>> GetPostCommentsAsync(int id)
    {
        List<Comment> comments = await _unitOfWork.CommentRepository.GetAllAsync(n => n.PostId == id, "User.ProfileImage");
        List<CommentGetDto> commentGetDtos = _mapper.Map<List<CommentGetDto>>(comments);
        return commentGetDtos;
    }
}
