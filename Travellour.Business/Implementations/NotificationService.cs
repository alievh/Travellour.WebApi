using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Travellour.Business.DTOs.NotificationDTO;
using Travellour.Business.Helpers;
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
        List<Notification> notifications = await _unitOfWork.NotificationRepository.GetAllAsync(n => n.CreateDate, n => n.ReceiverId == userId, "Sender.ProfileImage", "Post");
        List<NotificationGetDto> notificationGetDtos = _mapper.Map<List<NotificationGetDto>>(notifications);
        for (int i = 0; i < notifications.Count; i++)
        {
            notificationGetDtos[i].FromCreateDate = notifications[i].CreateDate.GetTimeBetween();
        }
        return notificationGetDtos;
    }

    public async Task<List<NotificationGetDto>> GetPaginationNotificationAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<Notification> notifications = await _unitOfWork.NotificationRepository.PaginationAsync(n => n.CreateDate, n => n.ReceiverId == userId,0, 3, "Sender.ProfileImage", "Post");
        List<NotificationGetDto> notificationGetDtos = _mapper.Map<List<NotificationGetDto>>(notifications);
        for (int i = 0; i < notifications.Count; i++)
        {
            notificationGetDtos[i].FromCreateDate = notifications[i].CreateDate.GetTimeBetween();
        }
        return notificationGetDtos;
    }

    public async Task CreateNotificationAsync(NotificationCreateDto notificationCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Post post = await _unitOfWork.PostRepository.GetAsync(n => n.Id == notificationCreateDto.PostId, "Images");
        if(notificationCreateDto.Message != "liked your")
        {
            Notification notification = _mapper.Map<Notification>(notificationCreateDto);
            notification.SenderId = userId;
            notification.Post = post;
            notification.CreateDate = DateTime.UtcNow.AddHours(4);
            notification.NotificationStatus = Core.Entities.Enum.NotificationStatus.UnChecked;
            await _unitOfWork.NotificationRepository.CreateAsync(notification);
        } else
        {
            Notification checkNotification = await _unitOfWork.NotificationRepository.GetAsync(n => n.SenderId == userId && n.Post == post && n.ReceiverId == notificationCreateDto.ReceiverId && n.Message == "liked your");
            if (checkNotification == null)
            {
                Notification notification = _mapper.Map<Notification>(notificationCreateDto);
                notification.SenderId = userId;
                notification.Post = post;
                notification.CreateDate = DateTime.UtcNow.AddHours(4);
                notification.NotificationStatus = Core.Entities.Enum.NotificationStatus.UnChecked;
                await _unitOfWork.NotificationRepository.CreateAsync(notification);
            }
        }
    }

    public async Task ChangeNotificationSatatusAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Notification notification = await _unitOfWork.NotificationRepository.GetAsync(n => n.ReceiverId == userId && n.Id == id);
        notification.NotificationStatus = Core.Entities.Enum.NotificationStatus.Checked;
        await _unitOfWork.NotificationRepository.UpdateAsync(notification);
    }

    
}
