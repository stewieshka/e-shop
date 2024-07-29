using Grpc.Core;
using IdentityService.Application.Authentication.Login;
using IdentityService.Application.Authentication.Register;
using MediatR;

namespace IdentityService.Grpc;

public class IdentityService(ISender sender) : Identity.IdentityBase
{
    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        var command = new RegisterCommand(request.Email, request.Username, request.Password);

        var result = await sender.Send(command);

        return result;
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await sender.Send(command);

        return result;
    }
}