using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Travellour.Business.DTOs.EventDTO;
using Travellour.Business.Exceptions;
using Travellour.Business.Helpers;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostEnvironment _hostEnvironment;

    public EventService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<List<EventGetDto>> GetAllAsync()
    {
        List<Event> events = await _unitOfWork.EventRepository.GetAllAsync(n => n.CreateDate, n => !n.IsDeleted, "Images", "EventMembers");
        if (events is null) throw new NullReferenceException();
        List<EventGetDto> eventGetDtos = _mapper.Map<List<EventGetDto>>(events);
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].Images != null)
            {
                List<string> imageUrls = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var image in events[i].Images)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    imageUrls.Add(image.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                eventGetDtos[i].ImageUrls = imageUrls;
            }
        }
        return eventGetDtos;
    }

    public async Task<List<EventGetDto>> GetJoinedEventsAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        List<Event> events = await _unitOfWork.EventRepository.GetAllAsync(n => n.CreateDate, n => n.EventMembers.Contains(appUser), "Images", "EventMembers");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (events is null) throw new NullReferenceException();
        List<EventGetDto> eventGetDtos = _mapper.Map<List<EventGetDto>>(events);
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].Images != null)
            {
                List<string> imageUrls = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var image in events[i].Images)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    imageUrls.Add(image.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                eventGetDtos[i].ImageUrls = imageUrls;
            }
        }
        return eventGetDtos;
    }

    public async Task CreateAsync(EventCreateDto eventCreateDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Event eventDb = _mapper.Map<Event>(eventCreateDto);
        eventDb.CreateDate = DateTime.UtcNow.AddHours(4);
        eventDb.EventCreatorId = userId;
        eventDb.EventCreator = appUser;
        if (eventCreateDto.ImageFiles != null)
        {
            List<Image> images = new();
            foreach (var imageFile in eventCreateDto.ImageFiles)
            {
                Image image = new()
                {
                    ImageUrl = await imageFile.FileSaveAsync(_hostEnvironment.ContentRootPath, "Images")
                };
                await _unitOfWork.ImageRepository.CreateAsync(image);
                images.Add(image);
            }
            eventDb.Images = images;
        }
        await _unitOfWork.EventRepository.CreateAsync(eventDb);
    }

    public async Task JoinEventAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Event eventDb = await _unitOfWork.EventRepository.GetAsync(n => n.Id == id, "EventMembers");
        if (eventDb is null) throw new NotFoundException("Event Not Found!");
        eventDb.EventMembers?.Add(appUser);

        await _unitOfWork.EventRepository.UpdateAsync(eventDb);
    }

    public async Task LeaveEventAsync(int id)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        Event eventDb = await _unitOfWork.EventRepository.GetAsync(n => n.Id == id, "EventMembers");
        if (eventDb is null) throw new NotFoundException("Event Not Found!");
        eventDb.EventMembers?.Remove(appUser);

        await _unitOfWork.EventRepository.UpdateAsync(eventDb);
    }

    public async Task<List<EventGetDto>> SearchEventByName(string eventName)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        List<Event> events = await _unitOfWork.EventRepository.GetAllAsync(n => n.CreateDate, n => n.EventTitle.ToLower().StartsWith(eventName.Trim().ToLower()), "Images", "EventMembers");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (events is null) throw new NullReferenceException();
        List<EventGetDto> eventGetDtos = _mapper.Map<List<EventGetDto>>(events);
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].Images != null)
            {
                List<string> imageUrls = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var image in events[i].Images)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    imageUrls.Add(image.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                eventGetDtos[i].ImageUrls = imageUrls;
            }
        }
        return eventGetDtos;
    }

    public async Task DeleteEventAsync(int id)
    {
        Event eventDb = await _unitOfWork.EventRepository.GetAsync(n => n.Id == id, "Images");
        if(eventDb is null) throw new NotFoundException("Event Not Found!");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        foreach (var image in eventDb.Images)
        {
            await _unitOfWork.ImageRepository.DeleteAsync(image);
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        await _unitOfWork.EventRepository.DeleteAsync(eventDb);
    }
}
