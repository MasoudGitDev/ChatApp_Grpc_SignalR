using Apps.Auth.Accounts;
using Apps.Auth.Jwt;
using Domain.Auth.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Shared.Server.Dtos;
using Shared.Server.Exceptions;
using Shared.Server.Extensions;

namespace Infra.EFCore.Implementations.Accounts;
internal class AccountService(UserManager<AppUser> _userManager , IJwtService _jwtService) : IAccountService {
    public async Task<string> LoginAsync(LoginDto model) {
        var user = (await FindUserAsync(model.LoginName)).ThrowIfNull("UserName or Password is wrong");
        bool isValidUser = await _userManager.CheckPasswordAsync(user, model.Password);
        if(isValidUser is false) {
           throw AccountException.Create("UserName or Password is wrong");
        }
        return await _jwtService.GenerateAsync(user.Id);
    }
    public async Task<string> LoginByTokenAsync(string token , string userId) {
        return await _jwtService.GenerateAsync(Guid.Parse(userId));
    }
    public async Task<string> RegisterAsync(RegisterDto model) {
        await ThrowIfFoundUserAsync(model.Email! , model.UserName!);
        Guid userId = Guid.NewGuid();
        var user = AppUser.Create(model ,userId);
        var result =  await _userManager.CreateAsync(user);
        if(!result.Succeeded) {
            throw AccountException.Create(ErrorsToString(result.Errors));
        }
        result = await _userManager.AddPasswordAsync(user , model.Password);
        if(!result.Succeeded) {
            throw AccountException.Create(ErrorsToString(result.Errors));
        }
        return await _jwtService.GenerateAsync(userId);
    }

    //============================
    private static string ErrorsToString(IEnumerable<IdentityError> errors) {
        string result = string.Empty;
        foreach(var error in errors) {
            result += $"{error.Code} : {error.Description}\n";
        }
        return result;
    }
    private async Task<AppUser?> FindUserAsync(string loginName) {
        string loginType = loginName.Contains('@') ? LoginType.Email : LoginType.UserName;
        return loginType switch {
            LoginType.Email => await _userManager.FindByEmailAsync(loginName),
            LoginType.UserName => await _userManager.FindByNameAsync(loginName),
            _ => await _userManager.FindByNameAsync(loginName),
        };
    }
    private async Task ThrowIfFoundUserAsync(string email , string userName) {
        ( await _userManager.FindByEmailAsync(email) ).ThrowIfNotNull($"The <user> with email : <{email}> exist.");
        ( await _userManager.FindByNameAsync(userName) ).ThrowIfNotNull($"The <user> with userName : <{userName}> exist.");
    }
    internal class LoginType {
        public const string Email = "Email";
        public const string UserName ="UserName";
    }
}
