namespace Shared.Server.Models.Results;
public class AccountResult {
    public bool IsValid { get; private set; } = false;
    public string AccessToken { get; private set; } = "<invalid-token>";
    public List<MessageDescription> Errors { get; private set; } = [];

    public AccountResult() { }

    public AccountResult(string accessToken) {
        IsValid = true;
        AccessToken = accessToken;
        Errors = [];
    }
    public AccountResult(List<MessageDescription> errors) {
        Errors = errors;
    }
    public static AccountResult Create(string accessToken) => new(accessToken);
    public static AccountResult Error(List<MessageDescription> errors) => new(errors);
    public static AccountResult Error(MessageDescription error) => new([error]);
    public static AccountResult Empty => new();
}
