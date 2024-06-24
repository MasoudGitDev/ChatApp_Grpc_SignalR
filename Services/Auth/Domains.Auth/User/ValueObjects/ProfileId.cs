namespace Domains.Auth.User.ValueObjects;
public class ProfileId {
    public string Value { get; private set; } = String.Empty;

    public ProfileId(string profileId) {
        if(String.IsNullOrWhiteSpace(profileId)) {
            throw new ArgumentNullException(nameof(profileId));
        }
        Value = profileId;
    }

    public static implicit operator ProfileId(string profileId) => new(profileId);
    public static implicit operator string(ProfileId profileId) => profileId.Value;
}
