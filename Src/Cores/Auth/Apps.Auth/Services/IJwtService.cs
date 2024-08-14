using Shared.Server.Dtos.User;
using Shared.Server.Models.Results;

namespace Apps.Auth.Services;
public interface IJwtService {
    Task<AccountResult> GenerateAsync(UserTokenDto model);
    Task<AccountResult> EvaluateAsync(string accessToken , UserTokenDto model);
}
