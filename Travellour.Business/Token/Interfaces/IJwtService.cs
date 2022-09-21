using Travellour.Core.Entities;

namespace Travellour.Business.Token.Interfaces;

public interface IJwtService
{
    public string GetJwt(AppUser user, IList<string> roles);
}
