using MediatR;

namespace IdentityService.Application.Authentication.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;