using Grpc.Core;
using IdentityService.Domain;
using IdentityService.Persistence.Repositories;
using IdentityService.Services;
using MediatR;

namespace IdentityService.Application.Authentication.Register;

public class RegisterCommandHandler(UserRepository userRepository) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByEmailAsync(command.Email) is not null)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "User with entered email already exists"));
        }

        var hashedPassword = PasswordService.HashPassword(command.Password);

        var user = new User(Guid.NewGuid(), command.Email, command.Username, hashedPassword);

        var result = await userRepository.AddAsync(user);

        if (!result)
        {
            throw new NotImplementedException();
        }

        return new RegisterResponse
        {
            Success = true
        };
    }
}