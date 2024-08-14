namespace Shared.Server.Constants;  
public record MessageType(string Name) {
    public static MessageType Successful => new(nameof(Successful));
    public static MessageType Warning => new(nameof(Warning));
    public static MessageType Error => new(nameof(Error));
    public static MessageType None => new(nameof(None));

    public static implicit operator string(MessageType messageType) => messageType.Name;
}
