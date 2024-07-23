using Blazored.LocalStorage;
using Client.ChatApp.Protos;
using Client.ChatApp.Protos.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Client.ChatApp.Services;

internal sealed class AuthStateProvider(
    HttpClient _httpClient ,
    AccountRPCs.AccountRPCsClient _accountService ,
    UserCommandsRPCs.UserCommandsRPCsClient _onlineUserCommands ,
    ILocalStorageService _localStorage) : AuthenticationStateProvider{

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        try {
            var accessToken = (await _localStorage.GetItemAsStringAsync(_tokenStorageKey , _cancellationToken))
                .ThrowIfNullOrWhiteSpace("The <access-token> is invalid");
            var result = await _accountService.LoginByTokenAsync(new LoginByTokenReq(){AccessToken =accessToken });
            if(!result.IsValid) {
                return _invalidUser;
            }
            var claims = GetClaims(result.AccessToken);
            SetAuthHeader(result.AccessToken);
            Console.WriteLine("Token by AuthStateProvider : " + result.AccessToken);
            await CreateOnlineUserAsync();
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims , _authenticationType)));
        }
        catch(Exception ex) {
            Console.WriteLine(ex.ToString());
            return _invalidUser;
        }
    }
    public async Task SetStateAsync(string? accessToken = null) {
        var authState = _invalidUser;
        if(accessToken is null) {
            await _localStorage.RemoveItemAsync(_tokenStorageKey , _cancellationToken);
        }
        else {
            await _localStorage.SetItemAsStringAsync(_tokenStorageKey , accessToken);
            var claims = GetClaims(accessToken);
            authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims , _authenticationType)));
        }
        SetAuthHeader(accessToken);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    //===============privates
    private readonly AuthenticationState _invalidUser = new(new ClaimsPrincipal(new ClaimsIdentity()));
    private readonly string _tokenStorageKey = "chat_app_token_id";
    private readonly CancellationToken _cancellationToken = new();
    private readonly string _authenticationType = "Bearer";

    private void SetAuthHeader(string? token = "<invalid-token") {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , token);
    }
    private static List<Claim> GetClaims(string accessToken) {
        var claims = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims.ToList();
        return claims;
    }

    private async Task CreateOnlineUserAsync() {
        var result = await _onlineUserCommands.CreateOrUpdateAsync(new Empty());
        if(result.IsSuccessful) {
            Console.WriteLine("OnlineUser - CreateOrUpdate : " + result.Messages.FirstOrDefault());
        }
        else {
            Console.WriteLine("OnlineUser - CreateOrUpdate : BadRequest");
        }
    }
}
