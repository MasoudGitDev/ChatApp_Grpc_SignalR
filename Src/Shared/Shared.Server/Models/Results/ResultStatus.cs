namespace Shared.Server.Models.Results;

public interface IResultStatus {
    bool IsSuccessful { get; init; } 
    List<MessageDescription> Messages { get; init; }
}

public record class ResultStatus(bool IsSuccessful , List<MessageDescription> Messages):IResultStatus;
public record class ResultStatus<T>(bool IsSuccessful , List<MessageDescription> Messages, T? Model) : IResultStatus;

