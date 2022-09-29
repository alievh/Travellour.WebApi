using Travellour.Business.DTOs.Event;

namespace Travellour.Business.Interfaces;

public interface IEventService
{
    Task<List<EventGetDto>> GetAllAsync();
    Task CreateAsync(EventCreateDto eventCreateDto);
}
