namespace Shared.Server.Models.Results;
public class ChatMessageResult {
    public static Result Send => new(nameof(Send), "The new message has been sent successfully.");
    public static Result Delete => new(nameof(Delete) , "The message has been deleted successfully.");
    public static Result Edit => new(nameof(Edit) , "The message has been Edited successfully.");
    public static Result NotFound(string message) => new(nameof(NotFound), message);
}
