namespace Shared.Server.Dtos;
public class RegisterDto {
    public string Email { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string DisplayName { get; set; } = "New User";
    public string ProfileId { get; set; } = Guid.NewGuid().ToString();
}