namespace Shared.Server.Dtos;
public class ChatMessageDto {
    public Guid UserId { get; set; }
    public bool IsMyMessage { get; set; } = false;
    public string Message { get; set; } = String.Empty;
    public string LogoUrl { get; set; } = String.Empty;
}
