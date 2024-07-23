namespace Shared.Server.Models.Results;

public record class SuccessResults(string Code , string Message) {
    // for commands
    public static ResultStatus Removed(Guid id)
        => new(true , [MessageDescription.Create(nameof(Removed) , $"The record with id :<{id}> is removed successfully.")]);

    public static ResultStatus Created
        => new(true , [MessageDescription.Create(nameof(Created) , "The new record is created successfully.")]);

    public static ResultStatus Ok(string message)
        => new(true , [MessageDescription.Create("Ok" , message)]);

    // for queries
    public static ResultStatus<T> Ok<T>(T data) => new(true , [],data);
    public static ResultStatus<T> Ok<T>(string message , T data)
    => new(true , [MessageDescription.Create( "Ok" , message)] , data);
}
