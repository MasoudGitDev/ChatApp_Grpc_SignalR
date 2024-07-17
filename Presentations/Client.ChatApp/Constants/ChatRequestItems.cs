namespace Client.ChatApp.Constants;

public record ChatRequestTab(string TabName) {
    public static ChatRequestTab Received => new("Received");
    public static ChatRequestTab Sent => new("Sent");
}
