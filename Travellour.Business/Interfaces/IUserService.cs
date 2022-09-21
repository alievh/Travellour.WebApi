using Travellour.Business.DTOs.User;

namespace Travellour.Business.Interfaces;

public interface IUserService
{
    Task<UserGetDto> GetAsync(string id);
}
