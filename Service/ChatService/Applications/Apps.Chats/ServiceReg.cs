using Apps.Chats.Commands;
using Apps.Chats.Commands.Impls;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Chats;
public static class ServiceReg {

    public static IServiceCollection AddChatServices(this IServiceCollection services) {
        services.AddScoped<IChatItemCommands , ChatItemCommands>();
        services.AddScoped<IChatMessageCommands , ChatMessageCommands>();
        return services;
    }

    public static WebApplication UseChatService(this WebApplication app) {
        return app;
    }

}
