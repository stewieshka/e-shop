using MediatR;

namespace Application.Users.Commands.Register;

public record RegisterCommand(string Username, string Email, string Password) : IRequest;