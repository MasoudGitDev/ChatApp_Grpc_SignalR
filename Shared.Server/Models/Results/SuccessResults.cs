namespace Shared.Server.Models.Results;

public record class SuccessResults(string Code , string Message) {
    public static ResultStatus Removed(Guid id)
        => new(true , [MessageDescription.Create(nameof(Removed) , $"The record with id :<{id}> is removed successfully.")]);

    public static ResultStatus Created
        => new(true , [MessageDescription.Create(nameof(Created) , "The new record is created successfully.")]);

    public static ResultStatus Ok(string message)
      => new(true , [MessageDescription.Create(nameof(Ok) , message)]);
}
