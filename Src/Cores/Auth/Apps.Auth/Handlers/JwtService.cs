using Apps.Auth.Services;
using Microsoft.IdentityModel.Tokens;
using Shared.Server.Constants;
using Shared.Server.Dtos.User;
using Shared.Server.Models;
using Shared.Server.Models.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Apps.Auth.Handlers;
internal class JwtService(JwtSettingsModel model) : IJwtService {
    public async Task<AccountResult> EvaluateAsync(string accessToken , UserTokenDto model) {
        return await ReNewAsync(await GetClaims(accessToken) , model);
    }

    public async Task<AccountResult> GenerateAsync(UserTokenDto model) {
        return await WriteToken(model);
    }

    //===========privates
    private const string _alg = SecurityAlgorithms.HmacSha256Signature;
    private SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(model.SecureKey));
    private Task<AccountResult> WriteToken(UserTokenDto model) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim> {
            new(TokenKeys.UserId , model.Id.ToString()),
            new(TokenKeys.UserName , model.UserName) ,
            new(TokenKeys.DisplayName , model.DisplayName)
        };
        var securityToken = tokenHandler.CreateToken(DescribeToken(claims));
        return Task.FromResult(AccountResult.Create(tokenHandler.WriteToken(securityToken)));

    }
    private SecurityTokenDescriptor DescribeToken(List<Claim> claims)
        => new() {
            SigningCredentials = new(SymmetricSecurityKey , _alg) ,
            Subject = new(claims) ,
            Issuer = model.Issuer ,
            Audience = model.Audience ,
            IssuedAt = DateTime.UtcNow ,
            NotBefore = DateTime.UtcNow ,
            Expires = DateTime.UtcNow.AddMinutes(model.ExpireMinuteNumber) ,
        };

    private static Task<List<Claim>> GetClaims(string accessToken)
        => Task.FromResult(new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims.ToList());

    private async Task<AccountResult> ReNewAsync(List<Claim> claims , UserTokenDto model) {
        var userIdentifier = claims.Where(x=> x.Type == TokenKeys.UserId).FirstOrDefault()?.Value;
        if(userIdentifier is null) {
            return AccountResult.Error(MessageDescription.Create("JWT-Error" , "The <UserIdentifier> is invalid."));
        }
        if(userIdentifier != model.Id.ToString()) {
            return AccountResult.Error(MessageDescription.Create("JWT-Error" , "This Token not belong to you!"));
        }
        return await GenerateAsync(model);
    }
}
