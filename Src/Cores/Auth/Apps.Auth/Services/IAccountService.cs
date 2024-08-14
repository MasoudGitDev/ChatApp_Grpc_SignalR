using Shared.Server.Dtos;
using Shared.Server.Models.Results;

namespace Apps.Auth.Services;
public interface IAccountService {
    Task<AccountResult> LoginAsync(LoginDto model);
    Task<AccountResult> LoginByTokenAsync(string accessToken);
    Task<AccountResult> RegisterAsync(RegisterDto model);
}
