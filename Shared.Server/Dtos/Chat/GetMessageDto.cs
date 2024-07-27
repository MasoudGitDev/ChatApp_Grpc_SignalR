namespace Shared.Server.Dtos.Chat;
public class GetMessageDto
{
    public Guid Id { get; set; }
    public Guid ChatItemId { get; set; }
    public Guid SenderId { get; set; }
    public bool IsMyMessage { get; set; } = false;
    public string Content { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public bool IsSent { get; set; }
    public bool IsSeen { get; set; }
}