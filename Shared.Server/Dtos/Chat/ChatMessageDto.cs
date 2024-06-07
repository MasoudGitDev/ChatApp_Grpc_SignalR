namespace Shared.Server.Dtos.Chat;
public class MessageDto
{
    public Guid MessageId { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public bool IsMyMessage { get; set; } = false;
    public string Content { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}
