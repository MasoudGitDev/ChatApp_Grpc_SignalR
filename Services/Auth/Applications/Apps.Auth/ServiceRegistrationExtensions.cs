using Apps.Auth.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Auth;
public static class ServiceRegistrationExtensions {
    public static IServiceCollection AddAuthService(this IServiceCollection services) {
        services.AddScoped<IJwtService , JwtService>();
        return services;
    }

}
