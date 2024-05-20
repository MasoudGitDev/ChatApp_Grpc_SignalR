using Apps.Auth.Constants;
using Microsoft.IdentityModel.Tokens;
using Shared.Server.Exceptions;
using Shared.Server.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shared.Server.Models;

namespace Apps.Auth.Jwt;

public interface IJwtService {
    Task<string> GenerateAsync(Guid userId);
    Task<string> EvaluateAsync(string token , string userId);
}

internal class JwtService(JwtSettingsModel model) : IJwtService {

    private readonly SymmetricSecurityKey _symmetricSecurityKey =new(Encoding.UTF8.GetBytes(model.SecureKey));
    private readonly string _alg = SecurityAlgorithms.HmacSha256Signature;

    public async Task<string> EvaluateAsync(string token , string userId) {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userIdValue = jwtSecurityToken.Claims.Where(x => x.Type == TokenKeys.UserId).FirstOrDefault()?.Value
            .ThrowIfNullOrWhiteSpace("<user-id> claim value can not be NullOrWhiteSpace.")  ;
        if(userId != userIdValue) {
            JwtException.Create("Your jwt token is invalid.");
        }
        return await GenerateAsync(Guid.Parse(userId));
    }

    public Task<string> GenerateAsync(Guid userId) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = (tokenHandler.CreateToken(DescribeToken(userId)))
            .ThrowIfNull("<securityToken> can not be null.");
        if(!tokenHandler.CanWriteToken) {
            JwtException.Create("System can not write token.");
        }
        return Task.FromResult(tokenHandler.WriteToken(securityToken));
    }


    //===============================
    private SecurityTokenDescriptor DescribeToken(Guid userId) {
        var claims = new List<Claim>(){
            new(TokenKeys.UserId , userId.ToString())
        };
        return new SecurityTokenDescriptor() {
            SigningCredentials = new SigningCredentials(_symmetricSecurityKey , _alg) ,
            Subject = new ClaimsIdentity(claims) ,
            Audience = model.Audience ,
            Issuer = model.Issuer ,
            IssuedAt = DateTime.UtcNow ,
            NotBefore = DateTime.UtcNow ,
            Expires = DateTime.UtcNow.AddMinutes(model.ExpireMinuteNumber)
        };
    }



}
