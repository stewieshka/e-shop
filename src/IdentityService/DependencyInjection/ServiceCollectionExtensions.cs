using System.Reflection;
using IdentityService.Persistence.Repositories;

namespace IdentityService.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
    
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<UserRepository>();
        
        return services;
    }
}