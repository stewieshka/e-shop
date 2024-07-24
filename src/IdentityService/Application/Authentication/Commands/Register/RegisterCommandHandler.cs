using Grpc.Core;
using IdentityService.DAL;
using IdentityService.Domain.Users;
using IdentityService.Services;
using MediatR;

namespace IdentityService.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    UserRepository userRepository, PasswordHasher passwordHasher) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // Проверка на уникальность почты
        if (await userRepository.GetByEmailAsync(command.Email) is not null)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A user with the entered email address already exists"));
        }

        var user = new User(Guid.NewGuid(), command.Email, command.Username, passwordHasher.Generate(command.Password));

        await userRepository.AddAsync(user);

        return new RegisterResponse { Success = true, Message = "User registered successfully" };
    }
}