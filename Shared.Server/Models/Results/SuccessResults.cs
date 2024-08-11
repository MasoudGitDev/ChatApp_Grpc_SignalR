namespace Shared.Server.Models.Results;

public record class SuccessResults(string Code , string Message) {
    // for commands
    public static ResultStatus Ok(string message)
        => new(true , [MessageDescription.Create("Ok" , message)]);

    // for queries
    public static ResultStatus<T> Ok<T>(T data) 
        => new(true , [],data);
    public static ResultStatus<T> Ok<T>(string message , T data)
    => new(true , [MessageDescription.Create( "Ok" , message)] , data);
}
