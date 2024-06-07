namespace Shared.Server.Models.Results;
public record class Result(string Code, string Message)
{
    public static Result Ok => new("Ok", "Every think is ok.");
    public static Result Cancel => new("Cancel", "The operations was canceled.");
    public static Result Error(string message) => new("Error", "Some thing is wrong.");
}
