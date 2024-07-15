namespace Shared.Server.Models.Results;

public record class ResultStatus(bool IsSuccessful , List<MessageDescription> Messages);
