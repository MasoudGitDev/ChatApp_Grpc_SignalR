using Domains.Auth.User.Aggregate;

namespace Domains.Chats.Requests.Aggregate;

/// <summary>
/// The Person-1 must only can send a request to Person-2 , when no one of each them is not a 'contact' member for other!
/// </summary>
public class ChatRequest {

    
    public Guid RequesterId { get; private set; }
    public Guid ReceiverId { get; private set; }

    public DateTime RequestedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// If The Request has Blocked by Receiver , The Requester must be not able to see his/her request!
    /// </summary>
    public bool IsBlockedByReceiver { get; set; } = false;



    //========== relations
    public AppUser Requester { get; private set; } = null!;
    public AppUser Receiver { get; private set; } = null!;



}
