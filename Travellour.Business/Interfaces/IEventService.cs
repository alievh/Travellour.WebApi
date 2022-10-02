using Travellour.Business.DTOs.EventDTO;

namespace Travellour.Business.Interfaces;

public interface IEventService
{
    Task<List<EventGetDto>> GetAllAsync();
    Task CreateAsync(EventCreateDto eventCreateDto);
}
