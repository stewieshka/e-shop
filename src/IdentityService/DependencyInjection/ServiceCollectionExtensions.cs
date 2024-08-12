using System.Reflection;
using IdentityService.Jwt;
using IdentityService.Persistence.Repositories;

namespace IdentityService.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.AddScoped<JwtProvider>();
        
        return services;
    }
    
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<UserRepository>();
        
        return services;
    }
}