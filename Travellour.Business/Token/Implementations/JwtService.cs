using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travellour.Business.Token.Interfaces;
using Travellour.Core.Entities;

namespace Travellour.Business.Token.Implementations;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GetJwt(AppUser user, IList<string> roles)
    {
        List<Claim> claims = new()
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:securityKey").Value));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new(
            issuer: _config.GetSection("Jwt:issuer").Value,
            audience: _config.GetSection("Jwt:audience").Value,
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
