using Grpc.Core;
using IdentityService.DAL;
using IdentityService.Jwt;
using IdentityService.Services;
using MediatR;

namespace IdentityService.Application.Authentication.Commands.Login;

public class LoginCommandHandler(
    UserRepository userRepository, 
    PasswordHasher passwordHasher,
    JwtProvider jwtProvider) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var user = await userRepository.GetByEmailAsync(command.Email);

        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User was not found"));
        }

        var isValidPassword = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!isValidPassword)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Wrong password"));
        }

        var accessToken = jwtProvider.GenerateToken(user);
        
        return new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            AccessToken = accessToken
        };
    }
}