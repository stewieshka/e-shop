using MediatR;

namespace Application.Users.Commands.Refresh;

public record RefreshCommand(
    string RefreshToken, string UserAgent, string Ip) : IRequest<RefreshResponse>;