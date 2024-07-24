namespace Shared.Server.Dtos;
public class ChatAccountDto {
    public Guid UserId { get; set; }
    public string LogoUrl { get; set; } = String.Empty;
    public string FullName { get; set; } = String.Empty;
    public int UnReadMessages { get; set; } = 0;


}
