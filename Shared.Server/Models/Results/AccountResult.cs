namespace Shared.Server.Models.Results;
public class AccountResult {
    public bool IsValid { get; private set; } = false;
    public string AccessToken { get; private set; } = "<invalid-token>";
    public List<CodeMessage> Errors { get; private set; } = [];

    public AccountResult() { }

    public AccountResult(string accessToken) {
        IsValid = true;
        AccessToken = accessToken;
        Errors = [];
    }
    public AccountResult(List<CodeMessage> errors) {
        Errors = errors;
    }
    public static AccountResult Create(string accessToken) => new(accessToken);
    public static AccountResult Error(List<CodeMessage> errors) => new(errors);
    public static AccountResult Error(CodeMessage error) => new([error]);
    public static AccountResult Empty => new();
}
