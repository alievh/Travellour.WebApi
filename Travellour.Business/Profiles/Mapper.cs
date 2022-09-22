using AutoMapper;
using Travellour.Business.DTOs.Post;
using Travellour.Business.DTOs.User;
using Travellour.Core.Entities;

namespace Travellour.Business.Profiles;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<AppUser, UserGetDto>()
            .ForMember(c => c.ProfileImage, c => c.Ignore());
        CreateMap<Post, PostGetDto>();
        CreateMap<PostCreateDto, Post>();
    }
}
