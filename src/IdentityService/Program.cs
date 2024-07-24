using IdentityService.DAL;
using IdentityService.GrpcServices;
using IdentityService.Jwt;
using IdentityService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<AppDbContext>(cfg =>
{
    cfg.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();

app.MapGrpcService<AuthService>();

app.MapGet("/", () => "It works!");

app.Run();
