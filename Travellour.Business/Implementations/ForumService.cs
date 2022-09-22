using AutoMapper;
using Travellour.Business.DTOs.Forum;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class ForumService : IForumService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ForumService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        return forumGetDto;
    }
}
