using Apps.Chats.Commands;
using Apps.Chats.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Chats;
public static class ServiceReg {

    public static IServiceCollection AddChatServices(this IServiceCollection services) {
        services.AddScoped<IChatMessageCommands , ChatMessageCommands>();
        return services;
    }

}
