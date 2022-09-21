using AutoMapper;
using Microsoft.AspNetCore.Http;
using Travellour.Business.DTOs.User;
using Travellour.Business.Interfaces;
using Travellour.Core;
using Travellour.Core.Entities;

namespace Travellour.Business.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserGetDto> GetAsync(string id)
    {
        AppUser appUser = await _unitOfWork.UserRepository.GetAsync(u => u.Id == id, includes: "ProfileImage");

        if (appUser is null)
        {
            throw new NullReferenceException();
        }

        UserGetDto userDto = _mapper.Map<UserGetDto>(appUser);
        userDto.ProfileImage = appUser.ProfileImage is not null ? appUser.ProfileImage.ImageUrl : "";
        return userDto;
    }
}
