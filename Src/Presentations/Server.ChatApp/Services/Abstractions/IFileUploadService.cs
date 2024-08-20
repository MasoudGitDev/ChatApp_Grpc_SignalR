using Shared.Server.Models.Results;

namespace Server.ChatApp.Services.Abstractions;

public interface IFileUploadService {
    public string Name { get; }
    Task<ResultStatus<string>> UploadAsync(IFormFile file,string userId);
}
