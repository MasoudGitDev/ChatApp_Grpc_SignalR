namespace Domains.Chats.Shared;
public static class SharedExtensions {
    public static IChatUOW HasValue(this IChatUOW? chatUOW) {
        ArgumentNullException.ThrowIfNull(chatUOW);
        return chatUOW;
    }
}
