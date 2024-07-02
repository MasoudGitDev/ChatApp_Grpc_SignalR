using Domains.Auth.User.Aggregate;
using Mapster;
using Server.ChatApp.Protos;
using Shared.Server.Constants;
using Shared.Server.Constants.View;
using Shared.Server.Models;

namespace Server.ChatApp.GRPCHandlers.Results;

public class ContactResults {

    // ============================ Results   

    public static ContactResult InvalidProfileId(string profileId) => FailureResult(InvalidProfileIdMsg(profileId));
    public static ContactResult InvalidContact => FailureResult(InvalidContactMsg);
    public static ContactResult FoundInRequests(MessageDescription messageDescription , string chatRequestId)
        => FailureResult(messageDescription , chatRequestId: chatRequestId);
    public static ContactResult FoundInContacts(AppUser userByProfileId)
    => SuccessResult(CreateContact(userByProfileId));

    public static ContactResult NotFoundInContacts(string profileId , AppUser userByProfileId)
        => WarningResult(NotFoundInContactsMsg(profileId) , contactInfo: CreateContact(userByProfileId));

    public static ContactResult NotFoundContactId(string contactId)
        => FailureResult(NotFoundContactIdMsg(contactId));

    public static ContactResult SuccessfulDeletion => SuccessResult();

    //============
    private static ContactInfo CreateContact(AppUser user) => new() {
        UserId = user.Id.ToString() ,
        Description = "test description" ,
        ImageUrl = "test img url"
    };

    // ============================ Results
    private static ContactResult FailureResult(MessageDescription model ,
        ContactInfo? contactInfo = null , string? chatRequestId = null) {
        ContactResult result = new() {
            IsSuccessful = false ,
            ContactInfo = contactInfo ?? new() ,
            ChatRequestId = chatRequestId ?? Guid.Empty.ToString()
        };
        result.Messages.Add(model.Adapt<MessageInfo>());
        return result;
    }
    private static ContactResult WarningResult(MessageDescription model ,
       ContactInfo? contactInfo = null , string? chatRequestId = null) {
        ContactResult result = new() {
            IsSuccessful = false ,
            ContactInfo = contactInfo ?? new() ,
            ChatRequestId = chatRequestId ?? Guid.Empty.ToString()
        };
        result.Messages.Add(model.Adapt<MessageInfo>());
        return result;
    }
    public static ContactResult SuccessResult(ContactInfo info) {
        ContactResult result = new() { IsSuccessful = false , ContactInfo = info };
        result.Messages.Add(new MessageInfo() { Code = "Ok" , Description = "Ok" , Type = MessageType.Successful });
        return result;
    }
    public static ContactResult SuccessResult() => new() { IsSuccessful = true , ContactInfo = new() };
    //==================
    public static MessageDescription InvalidProfileIdMsg(string id)
     => MessageDescription.Create(ProfileViewConstants.InvalidProfileId , $"The ProfileId : <{id}> is invalid." , MessageType.Error);

    public static MessageDescription InvalidContactMsg
        => MessageDescription.Create(ProfileViewConstants.InvalidContact , $"You can not add yourself to your contacts." , MessageType.Error);

    public static MessageDescription NotFoundInContactsMsg(string profileId)
        => MessageDescription.Create(ProfileViewConstants.NotFound ,
                $"The user with <profile-id> : <{profileId}> not exist in your contacts." , MessageType.Warning);

    public static MessageDescription NotFoundContactIdMsg(string id)
        => MessageDescription.Create("NotFoundContactId" , $"The ContactId : <{id}> not exist" , MessageType.Error);

}
