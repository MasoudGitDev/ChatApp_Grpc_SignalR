namespace Shared.Server.Dtos.Chat;
public class ChatItemDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReceiverId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public int UnReadMessages { get; set; } = 0;
}
