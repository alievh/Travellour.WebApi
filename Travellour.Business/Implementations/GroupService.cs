using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.Group;
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
        if(group.Image is not null)
        {
            groupGetDto.GroupImage = group.Image?.ImageUrl;
        }
        return groupGetDto;
    }

    public async Task CreateAsync(GroupCreateDto groupCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Group group = _mapper.Map<Group>(groupCreateDto);
        group.CreateDate = DateTime.UtcNow.AddHours(4);
        group.UserId = userId;
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

    public async Task<List<GroupGetDto>> GetAllAsyn()
    {
        List<Group> groups = await _unitOfWork.GroupRepository.GetAllAsync(includes: "Image");
        if (groups is null) throw new NullReferenceException();
        List<GroupGetDto> groupGetDtos = _mapper.Map<List<GroupGetDto>>(groups);
        for (int i = 0; i < groups.Count; i++)
        {
            groupGetDtos[i].GroupImage = groups[i].Image?.ImageUrl;
        }
        return groupGetDtos;
    }

    
}
