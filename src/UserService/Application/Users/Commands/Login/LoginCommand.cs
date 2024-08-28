using MediatR;

namespace Application.Users.Commands.Login;

public record LoginCommand(
    string Email, string Password, string Ip, string UserAgent) : IRequest<LoginResponse>;