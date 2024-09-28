using Application.Common.Interfaces;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Persistence.Repositories;
using Application.Common.Interfaces.Services;
using Domain.Sessions;
using Infrastructure.Configuration;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Database;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(
                configuration[DbConstants.DefaultConnectionStringPath]!));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ISessionRepository, SessionRepository>();

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        services.AddScoped<IJwtProvider, JwtProvider>();

        // в будущем затестить синглтон
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        return services;
    }
}