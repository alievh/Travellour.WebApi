using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.DTOs.NotificationDTO;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public NotificationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<List<NotificationGetDto>> GetAllNotificationAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Notification> notifications = await _unitOfWork.NotificationRepository.GetAllAsync(n => n.ReceiverId == userId, "Sender.ProfileImage", "Post");
        List<NotificationGetDto> notificationGetDtos = _mapper.Map<List<NotificationGetDto>>(notifications);
        return notificationGetDtos;
    }

    public async Task CreateNotificationAsync(NotificationCreateDto notificationCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Post post = await _unitOfWork.PostRepository.GetAsync(n => n.Id == notificationCreateDto.PostId, "Images");
        Notification checkNotification = await _unitOfWork.NotificationRepository.GetAsync(n => n.SenderId == userId && n.Post == post && n.ReceiverId == notificationCreateDto.ReceiverId && n.Message == "liked your");
        if (checkNotification == null)
        {
            Notification notification = _mapper.Map<Notification>(notificationCreateDto);
            notification.SenderId = userId;
            notification.Post = post;
            notification.NotificationStatus = Core.Entities.Enum.NotificationStatus.UnChecked;
            await _unitOfWork.NotificationRepository.CreateAsync(notification);
        }
    }

    public async Task ChangeNotificationSatatusAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Notification> notifications = await _unitOfWork.NotificationRepository.GetAllAsync(n => n.ReceiverId == userId);
        foreach (var notification in notifications)
        {
            notification.NotificationStatus = Core.Entities.Enum.NotificationStatus.Checked;
            await _unitOfWork.NotificationRepository.UpdateAsync(notification);
        }
    }


}
