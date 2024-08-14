namespace Shared.Server.Dtos.Chat;

public class SendMessageDto {
    public string Id { get; set; }
    public string ChatItemId { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string DisplayName { get; set; }
}