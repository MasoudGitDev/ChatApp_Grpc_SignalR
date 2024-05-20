using Apps.Auth.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Server.Dtos;

namespace Server.ChatApp.Controllers;
[Route("Api/[controller]")]
[ApiController]
public class AccountController(IAccountService _accountService , IAccountQueries _accountQueries)
    : AuthController(_accountQueries) {

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<string> RegisterAsync([FromBody]RegisterDto model) {
        return await _accountService.RegisterAsync(model);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<string> LoginAsync([FromBody] LoginDto model) {
        return await _accountService.LoginAsync(model);
    }

    [HttpPost("LoginByToken")]
    public async Task<string> LoginByTokenAsync([FromForm] string token) {
        return await _accountService.LoginByTokenAsync(token , (await GetUser()).Id.ToString() );
    }
}
