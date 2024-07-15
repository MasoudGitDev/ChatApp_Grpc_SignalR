namespace Shared.Server.Models.Results;

public record class ChatItemResult(string Code , string Message) {
    public static ResultStatus SameId
        => new(false , [MessageDescription.Create(nameof(SameId) , "You can not chat with yourself!")]);

    public static ResultStatus Founded
        => new(false , [MessageDescription.Create(nameof(Founded) , "There is a chat between you and the other person!")]);

    public static ResultStatus NotFound(string message)
        => new(false , [MessageDescription.Create(nameof(Created) , message)]);

    public static ResultStatus Removed(Guid id)
        => new(true , [MessageDescription.Create(nameof(Removed) , $"The record with id :<{id}> is removed successfully.")]);

    public static ResultStatus Created
        => new(true , [MessageDescription.Create(nameof(Created) , "The new record is created successfully.")]);


}
