using Travellour.Business.DTOs.Event;

namespace Travellour.Business.Interfaces;

public interface IEventService
{
    Task<EventGetDto> GetAsync(int id);
    Task<List<EventGetDto>> GetAllAsync();
}
