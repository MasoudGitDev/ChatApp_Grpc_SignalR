using Apps.Auth.Constants;
using Apps.Auth.Services;
using Domains.Auth.User.Aggregate;
using Microsoft.AspNetCore.Identity;
using Shared.Server.Dtos;
using Shared.Server.Exceptions;
using Shared.Server.Extensions;
using Shared.Server.Models;
using Shared.Server.Models.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infra.EFCore.Implementations.Accounts;
internal class AccountService(UserManager<AppUser> _userManager , IJwtService _jwtService) : IAccountService {
    public async Task<AccountResult> LoginAsync(LoginDto model) {
        var user = (await FindUserAsync(model.LoginName));
        if(user is null) {
            return AccountResult.Error(MessageDescription.Create("Invalid-User" , "LoginName or Password is wrong."));
        }
        bool isValidUser = await _userManager.CheckPasswordAsync(user, model.Password);
        if(isValidUser is false) {
            return AccountResult.Error(MessageDescription.Create("Invalid-User" , "LoginName or Password is wrong."));
        }
        return await _jwtService.GenerateAsync(user.Id);
    }
    public async Task<AccountResult> LoginByTokenAsync(string accessToken) {
        try {
            // must evaluate the user here
            var userIdClaim = GetUserIdByClaims(await GetClaimsAsync(accessToken));
            //...
            // if every things is ok then :
            return await _jwtService.EvaluateAsync(accessToken , userIdClaim);
        }
        catch(Exception ex) {
            return AccountResult.Error(MessageDescription.Create("Error" , ex.Message));
        }
    }
    public async Task<AccountResult> RegisterAsync(RegisterDto model) {
        await ThrowIfFoundUserAsync(model.Email! , model.UserName!);
        var user = AppUser.Create(model);
        var result =  await _userManager.CreateAsync(user);
        if(!result.Succeeded) {
            throw AccountException.Create(ErrorsToString(result.Errors));
        }
        result = await _userManager.AddPasswordAsync(user , model.Password);
        if(!result.Succeeded) {
            throw AccountException.Create(ErrorsToString(result.Errors));
        }
        return await _jwtService.GenerateAsync(model.Id);
    }

    //============================
    private static Task<IEnumerable<Claim>> GetClaimsAsync(string token) { 
        JwtSecurityTokenHandler handler = new();
        if(!handler.CanReadToken(token)) {
            throw new Exception("Invalid-Token");
        }
       return Task.FromResult(handler.ReadJwtToken(token).Claims);
    }
    private static string GetUserIdByClaims(IEnumerable<Claim> claims) {
        return claims.Where(x=>x.Type == TokenKeys.UserId).FirstOrDefault()?.Value ?? string.Empty;
    }
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
