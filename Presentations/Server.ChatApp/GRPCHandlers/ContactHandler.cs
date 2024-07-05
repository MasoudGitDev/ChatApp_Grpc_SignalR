using Apps.Chats.UnitOfWorks;
using Domains.Auth.User.Aggregate;
using Domains.Chats.Requests.Aggregate;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Server.ChatApp.GRPCHandlers.Results;
using Server.ChatApp.Protos;
using Shared.Server.Constants;
using Shared.Server.Dtos.Dashboard;
using Shared.Server.Extensions;
using Server.ChatApp.Extensions;

namespace Server.ChatApp.GRPCHandlers;

[Authorize]
internal sealed class ContactHandler(IChatUOW _unitOfWork) : ContactRPCs.ContactRPCsBase {

    //======== Queries
    public override async Task<ContactResult> IsInContacts(ContactMsg request , ServerCallContext context) {
        // get profileId and check is valid
        var userByProfileId = await FindUserByProfileIdAsync(request.ProfileId);
        if(userByProfileId is null) {
            return ContactResults.InvalidProfileId(request.ProfileId);
        }
        // check is other-user is in my contact 
        Guid myId = await SharedMethods.GetMyIdAsync(context,_unitOfWork);
        var contactUser = await _unitOfWork.Queries.Contacts.IsInContactAsync(userByProfileId.Id ,myId);

        if(contactUser is null) {
            if(myId == userByProfileId.Id) {
                return ContactResults.InvalidContact;
            }
            var (requestStatus, chatRequestId) = await GetRequestStatusAsync(myId , userByProfileId.Id);
            if(requestStatus != ChatRequestStatus.NotFound) {
                return ContactResults.FoundInRequests(requestStatus , chatRequestId.ToString());
            }
            return ContactResults.NotFoundInContacts(request.ProfileId , userByProfileId);
        }
        return ContactResults.FoundInContacts(userByProfileId);
    }

    public override async Task<ContactItems> GetContacts(Empty request , ServerCallContext context) {
        ContactItems result = new();
        var myId = await SharedMethods.GetMyIdAsync(context , _unitOfWork);
        var contactItems = ( await _unitOfWork.Queries.Contacts.GetContacts(myId) ).AsRepeatedFields<ContactItem,ContactItemDto>();
        result.Items.AddRange(contactItems);
        return result;
    }


    //========= Commands

    public override async Task<ContactResult> Remove(RowMsg request , ServerCallContext context) {
        var model = await _unitOfWork.Queries.Contacts.FindAsync(request.RowId.AsGuid());
        if(model is null) {
            return ContactResults.NotFoundContactId(request.RowId);
        }
        return ContactResults.SuccessfulDeletion;
    }

    //======================privates   
    private async Task<AppUser?> FindUserByProfileIdAsync(string profileId) {
        return await _unitOfWork.Queries.Users.FindByProfileIdAsync(profileId);
    }
    private async Task<(ChatRequestStatus RequestStatus, Guid ChatReqeustId)> GetRequestStatusAsync(Guid myId , Guid otherUserId) {
        var findSameRequest = await _unitOfWork.Queries.ChatRequests.FindSameRequestAsync(myId, otherUserId);
        return (CalcRequestStatus(findSameRequest , myId), findSameRequest?.Id ?? Guid.Empty);
    }
    private static ChatRequestStatus CalcRequestStatus(ChatRequest? findSameRequest , Guid myId) {
        if(findSameRequest is null) {
            return ChatRequestStatus.NotFound;
        }
        if(!findSameRequest.IsBlockedByReceiver && findSameRequest.ReceiverId == myId) {
            return ChatRequestStatus.Confirm;
        }
        if(findSameRequest.IsBlockedByReceiver && findSameRequest.ReceiverId == myId) {
            return ChatRequestStatus.UnBlock;
        }
        if(findSameRequest.IsBlockedByReceiver && findSameRequest.RequesterId == myId) {
            return ChatRequestStatus.BlockedByReceiver;
        }
        if(findSameRequest.RequesterId == myId) {
            return ChatRequestStatus.WaitForAccept;
        }
        return ChatRequestStatus.NotFound;
    }
}
