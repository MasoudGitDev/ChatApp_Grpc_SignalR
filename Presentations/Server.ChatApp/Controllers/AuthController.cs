using Apps.Auth.Accounts;
using Domain.Auth.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Server.Extensions;

namespace Server.ChatApp.Controllers;
[ApiController]
[Authorize]
public class AuthController(IAccountQueries _accountQueries) : ControllerBase {

    protected string GetUserNameAsync() {
        if(User.Identity is null || !User.Identity.IsAuthenticated) {
            throw new Exception("You are not authorized.");
        }
        return User.Identity.Name.ThrowIfNullOrWhiteSpace("The <user-name> can not be NullOrWhiteSpace.");
    }
    protected async Task<AppUser> GetUser() {
        return (await _accountQueries.User.FindByUserNameAsync(GetUserNameAsync())).ThrowIfNull("You are not authorized.");
    }
}
