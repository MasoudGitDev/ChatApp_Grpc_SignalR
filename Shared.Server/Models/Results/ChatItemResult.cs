namespace Shared.Server.Models.Results;

public record class ChatItemResult(string Code , string Message) {
    public static Result Created => new("Ok" , "The new chat was created successfully.");
    public static Result SameId => new("SameId" , "You can not chat with yourself!");
    public static Result Duplicate => new("Duplicate" , "There is a chat between you and the other person!");
    public static Result NotFound(string message) => new("NotFound" , message);
}
