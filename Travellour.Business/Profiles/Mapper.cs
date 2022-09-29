using AutoMapper;
using Travellour.Business.DTOs.Event;
using Travellour.Business.DTOs.Group;
using Travellour.Business.DTOs.Post;
using Travellour.Business.DTOs.User;
using Travellour.Core.Entities;

namespace Travellour.Business.Profiles;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<AppUser, UserGetDto>()
            .ForMember(c => c.ProfileImage, c => c.Ignore())
            .ForMember(c => c.CoverImage, c => c.Ignore());
        CreateMap<AppUser, FriendSuggestionDto>();
        CreateMap<Post, PostGetDto>()
            .ForMember(c => c.ImageUrls, c => c.Ignore());
        CreateMap<PostCreateDto, Post>();
        CreateMap<Group, GroupGetDto>();
        CreateMap<GroupCreateDto, Group>();
        CreateMap<Event, EventGetDto>()
            .ForMember(c => c.ImageUrls, c => c.Ignore());
        CreateMap<EventCreateDto, Event>();
    }
}
