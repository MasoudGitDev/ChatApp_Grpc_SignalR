namespace Apps.Auth.Services;
public interface ICaptchaService {
    Task<byte[]> CreateAsync(string text);
    Task<string> CreateContentAsync();
}
