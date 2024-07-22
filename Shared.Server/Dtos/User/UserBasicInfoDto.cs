namespace Shared.Server.Dtos.User;
public record UserBasicInfoDto(string ProfileId , string DisplayName , string ImageUrl = "") {
    public static UserBasicInfoDto New(string profileId , string displayName , string imageUrl = "")
        => new(profileId , displayName , imageUrl);
}