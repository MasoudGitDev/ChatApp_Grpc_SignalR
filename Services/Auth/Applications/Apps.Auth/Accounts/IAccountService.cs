using Shared.Server.Dtos;
using Shared.Server.Models.Results;

namespace Apps.Auth.Accounts;

public interface IAccountService {
    Task<AccountResult> RegisterAsync(RegisterDto model);
    Task<AccountResult> LoginAsync(LoginDto model);
    Task<AccountResult> LoginByTokenAsync(string token , string userId);
}

