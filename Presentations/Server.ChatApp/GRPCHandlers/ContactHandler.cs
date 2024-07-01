using Apps.Chats.UnitOfWorks;
using Domains.Auth.User.Aggregate;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.GRPCHandlers.Results;
using Server.ChatApp.Protos;
using Shared.Server.Constants;
using Shared.Server.Extensions;

namespace Server.ChatApp.GRPCHandlers;

[Authorize]
internal sealed class ContactHandler(IChatUOW _unitOfWork) : ContactRPCs.ContactRPCsBase {

    public override async Task<ContactResult> IsInContacts(ContactMsg request , ServerCallContext context) {
        var validUser = await FindUserByProfileIdAsync(request.ProfileId);
        if(validUser is null) {
            return ContactResults.FailureResult("Invalid-ProfileId" , $"The ProfileId : <{request.ProfileId}> is invalid.");
        }
        Guid myId = await SharedMethods.GetUserIdAsync(context,_unitOfWork);
        ContactInfo contactInfo = new(){
            UserId = validUser.Id.ToString() ,
            Description = "test user description" ,
            ImageUrl = "test image url"
        };
        var contactUser = await _unitOfWork.Queries.Contacts.IsInContactAsync(validUser.Id ,myId);
        if(contactUser is null) {
            if(myId == validUser.Id) {
                return ContactResults.FailureResult("MyId" , $"You can not add yourself to your contacts.");
            }
            var hasAnyRequest = await HasAnyChatRequestAsync(myId , validUser.Id);
            if(hasAnyRequest.Code != "Ok") {
                return ContactResults.FailureResult(hasAnyRequest.Code , hasAnyRequest.Description);
            }
            return ContactResults.WarningResult(
                "NotExist" ,
                $"The user with <profile-id> : <{request.ProfileId}> not exist in your contacts." ,
                contactInfo);
        }
        return ContactResults.SuccessResult(contactInfo);
    }

    public override async Task<ContactResult> Remove(RowMsg request , ServerCallContext context) {
        var model = await _unitOfWork.Queries.Contacts.FindAsync(request.RowId.AsGuid());
        if(model is null) {
            return ContactResults.FailureResult("NotExist" , $"The ContactId : <{request.RowId}> not exist");
        }
        return ContactResults.SuccessResult();
    }

    //======================privates   
    private async Task<AppUser?> FindUserByProfileIdAsync(string profileId) {
        return await _unitOfWork.Queries.Users.FindByProfileIdAsync(profileId);
    }
    private async Task<MessageInfo> HasAnyChatRequestAsync(Guid myId , Guid otherUserId) {
        MessageInfo messageInfo = new() { Code = "Ok" , Description = "Ok" , Type = MessageType.Successful};
        var findSameRequest = await _unitOfWork.Queries.ChatRequests.FindSameRequestAsync(myId, otherUserId);
        if(findSameRequest is not null) {
            var msg = "Confirm the Request";
            if(findSameRequest.RequesterId == myId) {
                msg = "Your request has not been confirmed yet.";
                return new() { Code = "Founded" , Description = msg , Type = MessageType.Error };
            }
            return new() { Code = "Confirm" , Description = msg , Type = MessageType.Error };
        }
        return messageInfo;
    }
}
