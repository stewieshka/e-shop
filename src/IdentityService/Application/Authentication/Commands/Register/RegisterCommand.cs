using MediatR;

namespace IdentityService.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Email, string Username, string Password) : IRequest<RegisterResponse>;