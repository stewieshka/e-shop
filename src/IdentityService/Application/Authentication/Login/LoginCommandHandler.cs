using Grpc.Core;
using IdentityService.Persistence.Repositories;
using IdentityService.Services;
using MediatR;

namespace IdentityService.Application.Authentication.Login;

public class LoginCommandHandler(UserRepository userRepository) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email);

        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User with entered email was not found"));
        }

        var passwordVerifyResult = PasswordService.Verify(command.Password, user.PasswordHash);

        if (!passwordVerifyResult)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid password"));
        }

        // TODO: Сделать генератор JWT
        var token = "new token";

        return new LoginResponse
        {
            Token = token
        };
    }
}