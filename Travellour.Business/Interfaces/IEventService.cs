﻿using Travellour.Business.DTOs.EventDTO;

namespace Travellour.Business.Interfaces;

public interface IEventService
{
    Task<List<EventGetDto>> GetAllAsync();
    Task<List<EventGetDto>> GetJoinedEventsAsync();
    Task<List<EventGetDto>> SearchEventByName(string eventName);
    Task CreateAsync(EventCreateDto eventCreateDto);
    Task JoinEventAsync(int id);
    Task LeaveEventAsync(int id);
}
