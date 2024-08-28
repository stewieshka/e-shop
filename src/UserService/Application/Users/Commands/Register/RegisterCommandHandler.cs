using Application.Common.Interfaces.Persistence.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Users;
using Grpc.Core;
using MediatR;

namespace Application.Users.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var checkUser = await userRepository.GetByEmailAsync(command.Email);

        if (checkUser is not null)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "User with entered email already exists"));
        }

        passwordHasher.Hash(command.Password, out var salt, out var hash);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Username = command.Username,
            PasswordSalt = salt.ToString(),
            PasswordHash = hash.ToString()
        };

        var result = await userRepository.CreateAsync(user);

        if (result < 1)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Error while creating a user"));
        }
    }
}