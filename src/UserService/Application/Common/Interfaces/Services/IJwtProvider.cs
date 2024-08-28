using Domain.Users;

namespace Application.Common.Interfaces.Services;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
}