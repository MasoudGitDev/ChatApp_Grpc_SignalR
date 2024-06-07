using Apps.Auth.Constants;
using Apps.Auth.Services;
using Microsoft.IdentityModel.Tokens;
using Shared.Server.Exceptions;
using Shared.Server.Models;
using Shared.Server.Models.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Apps.Auth.Handlers;
internal class JwtService(JwtSettingsModel model) : IJwtService {
    public async Task<AccountResult> EvaluateAsync(string accessToken , string userId) {
       return await ReNewAsync(await GetClaims(accessToken),userId);
    }

    public async Task<AccountResult> GenerateAsync(Guid userId) {
        return await WriteToken(userId);
    }

    //===========privates
    private const string _alg = SecurityAlgorithms.HmacSha256Signature;
    private SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(model.SecureKey));
    private Task<AccountResult> WriteToken(Guid userId) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim> {
            new(TokenKeys.UserId , userId.ToString())
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

    private async Task<AccountResult> ReNewAsync(List<Claim> claims , string userId) {
        var userIdentifier = claims.Where(x=> x.Type == TokenKeys.UserId).FirstOrDefault()?.Value;
        if(userIdentifier is null) {
            throw JwtException.Create("The <UserIdentifier> is invalid.");
        }
        if(userIdentifier != userId) {
            throw JwtException.Create("This Token not belong to you!");
        }
        return await GenerateAsync(Guid.Parse(userId));
    }
}
