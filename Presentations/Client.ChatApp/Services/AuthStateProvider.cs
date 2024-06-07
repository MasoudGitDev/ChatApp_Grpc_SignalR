using Blazored.LocalStorage;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Client.ChatApp.Services;

internal sealed class AuthStateProvider(
    HttpClient _httpClient ,
    ILocalStorageService _localStorage) : AuthenticationStateProvider {

    private readonly AuthenticationState _invalidUser = new(new ClaimsPrincipal(new ClaimsIdentity()));
    private readonly string _tokenStorageKey = "chat_app_token_id";
    private readonly CancellationToken _cancellationToken = new();
    private readonly string _authenticationType = "Bearer";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        try {
            var accessToken = (await _localStorage.GetItemAsStringAsync(_tokenStorageKey , _cancellationToken))
                .ThrowIfNullOrWhiteSpace("The <access-token> is invalid");
            var gRPCChannel = GrpcChannel.ForAddress("https://localhost:7001" , new GrpcChannelOptions(){
                HttpClient = _httpClient,
            });
            var accountService = new AccountRPCs.AccountRPCsClient(gRPCChannel);
            var result = accountService.LoginByTokenAsync(new LoginByTokenReq(){AccessToken =accessToken });

            var claims = GetClaims(accessToken);
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

    public void SetAuthHeader(string? token = "<invalid-token") {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , token);
    }

    private static List<Claim> GetClaims(string accessToken) {
        var claims = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims.ToList();
        return claims;
    }
    public static CallCredentials FromAccessToken(string token) {
        return CallCredentials.FromInterceptor((context , metadata) => {
            if(!string.IsNullOrEmpty(token)) {
                metadata.Add("Authorization" , $"Bearer {token}");
            }
            return Task.CompletedTask;
        });
    }
}
