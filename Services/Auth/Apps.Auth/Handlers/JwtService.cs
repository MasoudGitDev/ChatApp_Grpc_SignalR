using Apps.Auth.Services;
using Shared.Server.Models.Results;

namespace Apps.Auth.Handlers;
internal class JwtService : IJwtService {
    public Task<AccountResult> EvaluateAsync(string accessToken , string userId) {
        throw new NotImplementedException();
    }

    public Task<AccountResult> GenerateAsync(Guid userId) {
        throw new NotImplementedException();
    }
}
