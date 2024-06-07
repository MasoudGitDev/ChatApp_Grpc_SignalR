namespace Domains.Chat.ChatAggregate.Operations;
internal interface IChatItemCommands {
    Task CreateAsync(ChatItem chatItem);
}
