using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces.Services;
using Domain.Users;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateAccessToken(User user)
    {
        var signingCredintials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.Sha256);

        var jwtSecurityToken = new JwtSecurityToken(
            signingCredentials: signingCredintials,
            expires: DateTime.UtcNow.AddMinutes(15),
            claims: new[]
            {
                new Claim("userId", user.Id.ToString())
            });

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return accessToken;
    }
}