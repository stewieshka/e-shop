using Api.Grpc;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<UserService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

DbInitializer.Initialize(builder.Configuration[DbConstants.DefaultConnectionStringPath]!);

app.Run();
