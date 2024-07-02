using Client.ChatApp.Protos;

namespace Shared.Server.Constants.View;  
public class SharedViewCodes {
    public static List<MessageInfo> GetErrors(List<MessageInfo> messages)
       => messages.Where(x => x.Type == MessageType.Error).Select(x => x).ToList();

    // write better description...
    public static MessageInfo NotPossible => new() { Code = "NotPossible" , Description ="Some things is wrong." , Type = MessageType.Error };
    public static MessageInfo NotCompletedLoading => new() { Code = "NotCompletedLoading" , Description = "The page is not loading completely." , Type = MessageType.Error };

}
