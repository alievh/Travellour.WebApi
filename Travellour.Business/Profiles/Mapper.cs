using AutoMapper;
using Travellour.Business.DTOs.CommentDTO;
using Travellour.Business.DTOs.EventDTO;
using Travellour.Business.DTOs.ForumDTO;
using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.MessageDTO;
using Travellour.Business.DTOs.NotificationDTO;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Core.Entities;

namespace Travellour.Business.Profiles;

public class Mapper : Profile
{
    public Mapper()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        CreateMap<AppUser, UserGetDto>()
            .ForMember(c => c.ProfileImage, c => c.MapFrom(src => src.ProfileImage.ImageUrl))
            .ForMember(c => c.NotificationCount, c => c.MapFrom(src => src.Notifications.Count))
            .ForMember(c => c.CoverImage, c => c.MapFrom(src => src.CoverImage.ImageUrl));
        CreateMap<AppUser, UserProfileDto>()
            .ForMember(c => c.ProfileImage, c => c.MapFrom(src => src.ProfileImage.ImageUrl))
            .ForMember(c => c.CoverImage, c => c.MapFrom(src => src.CoverImage.ImageUrl))
            .ForMember(c => c.PostCount, c => c.MapFrom(src => src.Posts.Count))
            .ForMember(c => c.FriendCount, c => c.MapFrom(src => src.Friends.Count))
            .ForMember(c => c.Status, c => c.Ignore());
        CreateMap<AppUser, FriendSuggestionDto>()
            .ForMember(c => c.ImageUrl, c => c.MapFrom(src => src.ProfileImage.ImageUrl));
        CreateMap<Post, PostGetDto>()
            .ForMember(c => c.ImageUrls, c => c.Ignore())
            .ForMember(c => c.Comments, c => c.MapFrom(src => src.Comments))
            .ForMember(c => c.LikeCount, c => c.MapFrom(src => src.Likes.Count))
            .ForMember(c => c.CommentCount, c => c.MapFrom(src => src.Comments.Count));
        CreateMap<PostCreateDto, Post>();
        CreateMap<Group, GroupGetDto>()
            .ForMember(c => c.ProfileImage, c => c.MapFrom(src => src.ProfileImage.ImageUrl))
            .ForMember(c => c.CoverImage, c => c.MapFrom(src => src.CoverImage.ImageUrl));
        CreateMap<Group, GroupProfileDto>()
            .ForMember(c => c.ProfileImage, c => c.MapFrom(src => src.ProfileImage.ImageUrl))
            .ForMember(c => c.CoverImage, c => c.MapFrom(src => src.CoverImage.ImageUrl))
            .ForMember(c => c.MemberCount, c => c.MapFrom(src => src.GroupMembers.Count))
            .ForMember(c => c.PostCount, c => c.MapFrom(src => src.GroupPosts.Count));
        CreateMap<GroupCreateDto, Group>();
        CreateMap<Event, EventGetDto>()
            .ForMember(c => c.ImageUrls, c => c.Ignore());
        CreateMap<EventCreateDto, Event>();
        CreateMap<Forum, ForumGetDto>()
            .ForMember(c => c.Comments, c => c.MapFrom(src => src.Comments))
            .ForMember(c => c.CommentCount, c => c.MapFrom(src => src.Comments.Count));
        CreateMap<ForumCreateDto, Forum>();
        CreateMap<CommentCreateDto, Comment>();
        CreateMap<Comment, CommentGetDto>();
        CreateMap<NotificationCreateDto, Notification>();
        CreateMap<Notification, NotificationGetDto>();
        CreateMap<MessageDto, Message>();
        CreateMap<Message, GetMessage>();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
