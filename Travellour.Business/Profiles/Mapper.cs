using AutoMapper;
using Travellour.Business.DTOs.CommentDTO;
using Travellour.Business.DTOs.EventDTO;
using Travellour.Business.DTOs.ForumDTO;
using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Core.Entities;

namespace Travellour.Business.Profiles;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<AppUser, UserGetDto>()
            .ForMember(c => c.ProfileImage, c => c.Ignore())
            .ForMember(c => c.CoverImage, c => c.Ignore());
        CreateMap<AppUser, UserProfileDto>()
            .ForMember(c => c.ProfileImage, c => c.Ignore())
            .ForMember(c => c.CoverImage, c => c.Ignore())
            .ForMember(c => c.PostCount, c => c.Ignore())
            .ForMember(c => c.FriendCount, c => c.Ignore())
            .ForMember(c => c.Status, c => c.Ignore());
        CreateMap<AppUser, FriendSuggestionDto>();
        CreateMap<Post, PostGetDto>()
            .ForMember(c => c.ImageUrls, c => c.Ignore())
            .ForMember(c => c.LikeCount, c => c.Ignore())
            .ForMember(c => c.CommentCount, c => c.Ignore());
        CreateMap<PostCreateDto, Post>();
        CreateMap<Group, GroupGetDto>();
        CreateMap<Group, GroupProfileDto>()
            .ForMember(c => c.MemberCount, c => c.Ignore())
            .ForMember(c => c.PostCount, c => c.Ignore());
        CreateMap<GroupCreateDto, Group>();
        CreateMap<Event, EventGetDto>()
            .ForMember(c => c.ImageUrls, c => c.Ignore());
        CreateMap<EventCreateDto, Event>();
        CreateMap<Forum, ForumGetDto>()
            .ForMember(c => c.CommentCount, c => c.Ignore());
        CreateMap<ForumCreateDto, Forum>();
        CreateMap<CommentCreateDto, Comment>();
        CreateMap<Comment, CommentGetDto>();
    }
}
