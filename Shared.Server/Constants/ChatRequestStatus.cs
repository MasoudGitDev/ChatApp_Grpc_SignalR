using Shared.Server.Models;

namespace Shared.Server.Constants;

public record ChatRequestStatus(MessageDescription MessageDescription) {
    public static ChatRequestStatus NotFound
        => new(MessageDescription.Create(nameof(NotFound) , "<none>" , MessageType.Successful));

    public static ChatRequestStatus UnBlock 
        => new(MessageDescription.Create(nameof(UnBlock) , "Do you want to unblock the requester ?",MessageType.Warning));

    public static ChatRequestStatus BlockedByReceiver
        => new(MessageDescription.Create(nameof(BlockedByReceiver) , "The request has been blocked by the receiver." , MessageType.Warning));

    public static ChatRequestStatus WaitForAccept
        => new(MessageDescription.Create(nameof(WaitForAccept) , "Your request has not been confirmed yet." , MessageType.Warning));

    public static ChatRequestStatus Confirm
        => new(MessageDescription.Create(nameof(Confirm) , "Please confirm the request." , MessageType.Warning));

    public static implicit operator string(ChatRequestStatus chatRequestStatus) => chatRequestStatus.MessageDescription.Code;
    public static implicit operator MessageDescription(ChatRequestStatus chatRequestStatus)=> chatRequestStatus.MessageDescription;
}