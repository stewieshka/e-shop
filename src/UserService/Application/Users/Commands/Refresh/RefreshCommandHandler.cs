using Application.Common.Interfaces.Persistence.Repositories;
using Application.Common.Interfaces.Services;
using Grpc.Core;
using MediatR;

namespace Application.Users.Commands.Refresh;

public class RefreshCommandHandler(
    ISessionRepository sessionRepository,
    IUserRepository userRepository,
    IJwtProvider jwtProvider) : IRequestHandler<RefreshCommand, RefreshResponse>
{
    public async Task<RefreshResponse> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(command.RefreshToken, out var oldRefreshToken))
        {
            throw new RpcException(new Status(StatusCode.Unavailable, "Refresh token is invalid"));
        }
        
        var session = await sessionRepository.GetById(oldRefreshToken);

        if (session is null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated,
                "Session with entered refresh token was not found"));
        }

        if (session.ExpiresAt > DateTime.UtcNow)
        {
            var affectedRows = await sessionRepository.DeleteAsync(session.RefreshToken);

            if (affectedRows != 1)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Some error when deleting session"));
            }
            
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Refresh token is expired"));
        }
        
        if (session.UserAgent != command.UserAgent ||
            session.Ip != command.Ip)
        {
            var affectedRows = await sessionRepository.DeleteAsync(oldRefreshToken);

            if (affectedRows != 1)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Some error when deleting session"));
            }

            throw new RpcException(new Status(StatusCode.Aborted, "UserAgent or Ip is invalid, re-login"));
        }

        var user = await userRepository.GetById(session.UserId);

        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Some error when getting user by id"));
        }

        var newAccessToken = jwtProvider.GenerateAccessToken(user);

        var affectedRows2 = await sessionRepository.DeleteAsync(session.RefreshToken);

        if (affectedRows2 != 1)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Some error when deleting session"));
        }
        
        var newRefreshToken = Guid.NewGuid();

        session.RefreshToken = newRefreshToken;
        session.ExpiresAt = DateTime.UtcNow.AddDays(15);

        var affectedRows3 = await sessionRepository.CreateAsync(session);

        if (affectedRows3 != 1)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Some error when creating session"));
        }

        return new RefreshResponse(newAccessToken, newRefreshToken.ToString());
    }
}