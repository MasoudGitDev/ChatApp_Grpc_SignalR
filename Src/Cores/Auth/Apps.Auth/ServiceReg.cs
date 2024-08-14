using Apps.Auth.Handlers;
using Apps.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Auth;
public static class ServiceReg {

    public static IServiceCollection AddAuthServices(this IServiceCollection services) {
        services.AddScoped<IJwtService , JwtService>();
        return services;
    }

}
