using Shared.Server.Models.Results;

namespace Apps.Auth.Services;
public interface IJwtService {
    Task<AccountResult> GenerateAsync(Guid userId);
    Task<AccountResult> EvaluateAsync(string accessToken , string userId);
}
