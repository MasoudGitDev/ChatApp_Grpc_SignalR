using Shared.Server.Dtos;

namespace Apps.Auth.Accounts;

public interface IAccountService {
    Task<string> RegisterAsync(RegisterDto model);
    Task<string> LoginAsync(LoginDto model);
    Task<string> LoginByTokenAsync(string token , string userId);
}

