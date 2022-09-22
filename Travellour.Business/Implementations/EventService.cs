using AutoMapper;
using Travellour.Business.DTOs.Event;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EventService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EventGetDto> GetAsync(int id)
    {
        Event travellEvent = await _unitOfWork.EventRepository.GetAsync(n => n.Id == id && !n.IsDeleted, "EventCreator", "EventMembers");
        if (travellEvent is null) throw new NullReferenceException();
        EventGetDto eventGetDto = _mapper.Map<EventGetDto>(travellEvent);
        return eventGetDto;
    }

    public async Task<List<EventGetDto>> GetAllAsync()
    {
        List<Event> travellEvents = await _unitOfWork.EventRepository.GetAllAsync(n => !n.IsDeleted, "EventCreator", "EventMembers");
        if (travellEvents is null) throw new NullReferenceException();
        List<EventGetDto> eventGetDtos = _mapper.Map<List<EventGetDto>>(travellEvents);
        return eventGetDtos;
    }
}
