using Travellour.Business.DTOs.NotificationDTO;

namespace Travellour.Business.Interfaces;

public interface INotificationService
{
    Task<List<NotificationGetDto>> GetAllNotificationAsync();
    Task CreateNotificationAsync(NotificationCreateDto notificationCreateDto);
    Task ChangeNotificationSatatusAsync();
}
