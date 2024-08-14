using Apps.Auth.Services;

namespace Apps.Auth.Handlers;
internal class CaptchaService : ICaptchaService {
    private readonly string _baseText = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public Task<byte[]> CreateAsync(string text) {
        return default;
    }

    public Task<string> CreateContentAsync() {
        string randomText = new (Enumerable
            .Repeat(_baseText,6)
            .Select(str => str[new Random(str.Length).Next()])
            .ToArray()) ;
        return Task.FromResult(randomText);
    }
}
