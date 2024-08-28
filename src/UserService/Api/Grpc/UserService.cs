
using System.Security.Cryptography;
using Application.Users.Commands;
using Application.Users.Commands.Login;
using Application.Users.Commands.Refresh;
using Application.Users.Commands.Register;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Api.Grpc;

public class UserService(ISender sender) : User.UserBase
{
    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        var command = new RegisterCommand(request.Username, request.Email, request.Password);

        await sender.Send(command);

        return new RegisterResponse
        {
            Success = true
        };
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var command = new LoginCommand(request.Email, request.Password, request.Ip, request.UserAgent);

        var result = await sender.Send(command);

        return new LoginResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken.ToString(),
            ExpiresAt = Timestamp.FromDateTime(result.ExpiresAt)
        };
    }

    public override async Task<RefreshResponse> Refresh(RefreshRequest request, ServerCallContext context)
    {
        var command = new RefreshCommand(request.RefreshToken, request.UserAgent, request.Ip);

        var result = await sender.Send(command);

        return new RefreshResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        };
    }
}