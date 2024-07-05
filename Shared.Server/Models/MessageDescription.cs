using Shared.Server.Constants;

namespace Shared.Server.Models;
public record class MessageDescription {
    public MessageDescription()
    {
        
    }
    public string Code { get; private set; } = "<empty-code>";
    public string Description { get; private set; } = "<empty-description>";
    public MessageType Type { get; set; } = MessageType.None;
    public static MessageDescription Create(string code , string message) => new() { Code = code , Description = message };
    public static MessageDescription Create(string code , string message, MessageType type) => new() { Code = code , Description = message , Type = type };

    public static implicit operator string(MessageDescription message) => message.Code;
}
