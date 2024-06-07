using Apps.Auth.Accounts;
using Apps.Auth.Constants;
using Domain.Auth.UserAggregate;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Protos;
using Shared.Server.Dtos;
using Shared.Server.Exceptions;
using Shared.Server.Extensions;
using Shared.Server.Models.Results;

namespace Server.ChatApp.GRPCServices;

[Authorize]
public class GrpcAccountHandler(IAccountService _accountService , IUserQueries _userQueries) : AccountRPCs.AccountRPCsBase {
    public override Task<AccountResponse> Delete(DeleteReq request , ServerCallContext context) {
        return base.Delete(request , context);
    }

    [AllowAnonymous]
    public override async Task<AccountResponse> Login(LoginReq request , ServerCallContext context) {
        return ToAccountResponse(( await _accountService.LoginAsync(request.Adapt<LoginDto>()) ) , context);
    }



    [AllowAnonymous]
    public override async Task<AccountResponse> SignUp(RegisterReq request , ServerCallContext context) {
        return ToAccountResponse(await _accountService.RegisterAsync(request.Adapt<RegisterDto>()) , context);
    }

    public override async Task<AccountResponse> LoginByToken(LoginByTokenReq request , ServerCallContext context) {
        var accountResult = ( await _accountService.LoginByTokenAsync(request.AccessToken , (await GetUserAsync(context)).Id.ToString()));
        return ToAccountResponse(accountResult , context);
    }

    //======================privates
    private async Task<AppUser> GetUserAsync(ServerCallContext context) {
        var user = context.GetHttpContext().User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new AppException("InvalidUser" , "You are not authenticated.");
        }
        var userIdByClaim = user.Claims
            .Where(x=> x.Type == TokenKeys.UserId).FirstOrDefault()?.Value
            .ThrowIfNullOrWhiteSpace("The value of <userId> claim is invalid");
        _ = Guid.TryParse(userIdByClaim , out Guid userId);
        return ( await _userQueries.FindByIdAsync(userId) ).ThrowIfNull("Invalid-User");
    }

    private static AccountResponse ToAccountResponse(AccountResult accountResult , ServerCallContext context) {
        var response = accountResult.Adapt<AccountResponse>();
        response.Errors.AddRange(accountResult.Errors.Adapt<IEnumerable<Error>>());
        context.GetHttpContext().Response.Headers.Authorization = $"Bearer {response.AccessToken}";
        return response;
    }
}
