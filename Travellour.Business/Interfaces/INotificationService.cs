using Travellour.Business.DTOs.NotificationDTO;

namespace Travellour.Business.Interfaces;

public interface INotificationService
{
    Task<List<NotificationGetDto>> GetAllNotificationAsync();
    Task<List<NotificationGetDto>> GetPaginationNotificationAsync();
    Task CreateNotificationAsync(NotificationCreateDto notificationCreateDto);
    Task ChangeNotificationSatatusAsync(int id);
}
