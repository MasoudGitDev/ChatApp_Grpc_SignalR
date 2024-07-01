using Server.ChatApp.Protos;
using Shared.Server.Constants;

namespace Server.ChatApp.GRPCHandlers.Results;

public class ContactResults {
    public static ContactResult SuccessResult() => new() { IsSuccessful = true , ContactInfo = new() };
    public static ContactResult FailureResult(string code , string description) {
        ContactResult result = new() { IsSuccessful = false , ContactInfo = new() };
        result.Messages.Add(new MessageInfo() { Code = code , Description = description , Type = MessageType.Error });
        return result;
    }
    public static ContactResult WarningResult(string code , string description , ContactInfo info) {
        ContactResult result = new() { IsSuccessful = false , ContactInfo = info };
        result.Messages.Add(new MessageInfo() { Code = code , Description = description , Type = MessageType.Warning });
        return result;
    }
    public static ContactResult SuccessResult(ContactInfo info) {
        ContactResult result = new() { IsSuccessful = false , ContactInfo = info };
        result.Messages.Add(new MessageInfo() { Code = "Ok" , Description = "Ok" , Type = MessageType.Successful });
        return result;
    }
}
