using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Jwt;

public class JwtProvider(IOptions<JwtOptions> options)
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateToken(User user)
    {
        var claims = new Claim[]
        {
            new("userId", user.Id.ToString())
        };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.Sha256);
        
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiryHours));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}