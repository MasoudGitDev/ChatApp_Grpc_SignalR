using Shared.Server.Dtos;
using Shared.Server.Models.Results;

namespace Apps.Auth.Services;
public interface IAccountService {
    Task<AccountResult> LoginAsync(LoginDto model);
    Task<AccountResult> LoginByTokenAsync(string accessToken , string userId);
    Task<AccountResult> RegisterAsync(RegisterDto model);
}
