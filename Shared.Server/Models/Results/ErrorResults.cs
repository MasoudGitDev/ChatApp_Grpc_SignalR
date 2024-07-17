namespace Shared.Server.Models.Results;

public record class ErrorResults(string Code , string Message) {
    public static ResultStatus SameId
        => new(false , [MessageDescription.Create(nameof(SameId) , "You can not chat with yourself!")]);

    public static ResultStatus Founded(string message)
        => new(false , [MessageDescription.Create(nameof(Founded) , message)]);

    public static ResultStatus NotFound(string message)
        => new(false , [MessageDescription.Create(nameof(NotFound) , message)]);

    public static ResultStatus Removed(Guid id)
        => new(false , [MessageDescription.Create(nameof(Removed) , $"System can not remove the record with id :<{id}>.")]);

    public static ResultStatus Created
        => new(false , [MessageDescription.Create(nameof(Created) , "System can not create the new record.")]);
}

public record class ErrorResults<T>(string Code , string Message) {
    public static ResultStatus<T> Founded(string message , T? model =default)
        => new(false , [MessageDescription.Create(nameof(Founded) , message)],model);

    public static ResultStatus<T> NotFound(string message ,T? model=default)
        => new(false , [MessageDescription.Create(nameof(NotFound) , message)],model);
}