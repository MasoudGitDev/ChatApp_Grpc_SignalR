using UnitOfWorks.Abstractions;

namespace UnitOfWorks.Extensions;
public static class IChatUOWExtensions {
    public static IChatUOW HasValue(this IChatUOW? chatUOW) {
        ArgumentNullException.ThrowIfNull(chatUOW);
        return chatUOW;
    }
}
