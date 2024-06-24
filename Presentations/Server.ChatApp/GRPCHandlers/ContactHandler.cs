using Apps.Auth.Constants;
using Apps.Chats.UnitOfWorks;
using Domains.Auth.User.Aggregate;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.Protos;
using Shared.Server.Extensions;

namespace Server.ChatApp.GRPCHandlers;

[Authorize]
internal sealed class ContactHandler(IChatUOW _unitOfWork) : ContactRPCs.ContactRPCsBase {

    public override async Task<ResultMsg> IsInContacts(ContactMsg request , ServerCallContext context) {
        var validUser = await FindUserByProfileIdAsync(request.ProfileId);
        if (validUser is null)
        {
            return FailureResult("Invalid-ProfileId" , $"The ProfileId : <{request.ProfileId}> is invalid.");
        }
        var model = await _unitOfWork.Queries.Contacts.IsInContactAsync(validUser.Id , await GetUserIdAsync(context));
        return model == null
            ? FailureResult("NotExist" , $"The user with <profile-id> : <{request.ProfileId}> not exist in your contacts.")
            : SuccessResult;
    }

    public override async Task<ResultMsg> Remove(RowMsg request , ServerCallContext context) {
        var model = await _unitOfWork.Queries.Contacts.FindAsync(request.RowId.AsGuid());
        return model == null
            ? FailureResult("NotExist" , $"The ContactId : <{request.RowId}> not exist")
            : SuccessResult;
    }

    //======================privates   
    private async Task<AppUser?> FindUserByProfileIdAsync(string profileId) {
       return await _unitOfWork.Queries.Users.FindByProfileIdAsync(profileId);
    }

    private async Task<AppUser> GetUserAsync(ServerCallContext context) {
        var user = context.GetHttpContext().User;
        if(user is null || user.Identity is null || !user.Identity.IsAuthenticated) {
            throw new RpcException(Status.DefaultCancelled , "You are not authenticated.");
        }
        var userIdByClaim = user.Claims
            .Where(x => x.Type == TokenKeys.UserId).FirstOrDefault()?.Value
            ?? throw new RpcException(Status.DefaultCancelled ,"The value of <userId> claim is invalid");

        _ = Guid.TryParse(userIdByClaim , out Guid userId);
        return await _unitOfWork.Queries.Users.FindByIdAsync(userId)
            ?? throw new RpcException(Status.DefaultCancelled , "Invalid-User");
    }
    private async Task<Guid> GetUserIdAsync(ServerCallContext ctx) => ( await GetUserAsync(ctx) ).Id;
    private static ResultMsg SuccessResult => new() { IsSuccessful = true };
    private static ResultMsg FailureResult(string code , string description) {
        ResultMsg result = new() { IsSuccessful = false };
        result.Message.Add(new ErrorInfo() { Code = code , Description = description });
        return result;
    }
}
