namespace Apps.Auth.Constants;
public record TokenKeys(string Key) {
    public readonly static TokenKeys UserId =  new("UserId");

    public static implicit operator string(TokenKeys TokenKeys) => TokenKeys.Key;
}
