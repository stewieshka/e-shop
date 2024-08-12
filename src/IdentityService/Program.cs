
using IdentityService.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services
    .AddServices(builder.Configuration)
    .AddPersistence();

var app = builder.Build();

app.MapGrpcService<IdentityService.Grpc.IdentityService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();