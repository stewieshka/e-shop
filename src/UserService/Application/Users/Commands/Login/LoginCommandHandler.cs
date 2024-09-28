using System.Text;
using Application.Common.Interfaces.Persistence.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Sessions;
using Grpc.Core;
using MediatR;

namespace Application.Users.Commands.Login;

public class LoginCommandHandler
    (IUserRepository userRepository, IPasswordHasher passwordHasher, 
        IJwtProvider jwtProvider, ISessionRepository sessionRepository) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email);

        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User with entered email was not found"));
        }

        var passwordCheckResult = passwordHasher.Verify(command.Password, Convert.FromBase64String(user.PasswordSalt),
            Convert.FromBase64String(user.PasswordHash));

        if (!passwordCheckResult)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Wrong password"));
        }
        
        var accessToken = jwtProvider.GenerateAccessToken(user);
        
        var refreshToken = Guid.NewGuid();

        var session = new Session
        {
            RefreshToken = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(15),
            Ip = command.Ip,
            UserAgent = command.UserAgent
        };

        var affectedRows = await sessionRepository.CreateAsync(session);

        if (affectedRows != 1)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Some error"));
        }

        var loginResponse = new LoginResponse(accessToken, refreshToken, session.ExpiresAt);

        return loginResponse;
    }
}