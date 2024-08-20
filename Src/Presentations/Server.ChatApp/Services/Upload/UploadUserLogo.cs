using Server.ChatApp.Services.Abstractions;
using Shared.Server.Models.Results;

namespace Server.ChatApp.Services.Upload;

internal sealed class UploadUserLogo : IFileUploadService {
    public string Name => nameof(UploadUserLogo);
    public async Task<ResultStatus<string>> UploadAsync(IFormFile file , string userId) {
        var checkFileResult = CheckFile(file);
        if(!checkFileResult.IsSuccessful) {
            return checkFileResult;
        }
        return await SaveAsync(file , userId);
    }
    //====================== privates
    private static float _maxFileLength = 1024 * 1024;
    private static string[] _permittedImageExtensions => [".jpg" , ".jpeg" , ".png" , ".ico"];
    private static ResultStatus<string> CheckFile(IFormFile file) {
        if(file is null || file.Length <= 0) {
            return ErrorResults.Canceled<string>("Please select a file.");
        }
        if(file.Length > _maxFileLength) {
            return ErrorResults.Canceled<string>($"The length of the file ({file.Length}) must be less than or equal to 1 mb.");
        }
        string fileExtension = Path.GetExtension(file.FileName);
        if(!_permittedImageExtensions.Contains(fileExtension)) {
            return ErrorResults.Canceled<string>(
                $"The file extension : <{fileExtension}> must be in permitted image extensions (" +
                string.Join("," , _permittedImageExtensions) + ")");
        }
        return SuccessResults.Ok<string>("OK");
    }
    private static async Task<ResultStatus<string>> SaveAsync(IFormFile file,string userId) {
        try {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "AccountLogos" , userId , file.FileName);
            if(File.Exists(fullPath)) {
                return ErrorResults.Canceled<string>("Please try with unique name.");
            }
            string directoryName = Path.GetDirectoryName(fullPath) ?? string.Empty;
            if(string.IsNullOrWhiteSpace(directoryName)) {
                return ErrorResults.Canceled<string>("DirectoryName is empty!");
            }
            Directory.CreateDirectory(directoryName);
            using var stream = File.Create(fullPath);
            await file.CopyToAsync(stream);
            stream.Close();
            stream.Dispose();
            return SuccessResults.Ok<string>($"The file with name : {file.FileName} has been uploaded." , fullPath);
        }
        catch(Exception ex) {
            return ErrorResults.Canceled<string>(ex.Message);
        }
    }
}
