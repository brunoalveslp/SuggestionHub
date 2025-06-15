using Microsoft.Extensions.DependencyInjection;
using SuggestionHub.Infrastructure.Interfaces;
using SuggestionHub.Infrastructure.Repositories;
using SuggestionHub.Infrastructure.Services;


public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
