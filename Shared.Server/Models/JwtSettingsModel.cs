namespace Shared.Server.Models;
public class JwtSettingsModel {
    public string SecureKey { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty ;
    public string Audience { get; set; } = String.Empty;
    public double ExpireMinuteNumber { get; set; } = 60;
}
