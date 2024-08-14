namespace Shared.Server.Dtos.User;
public record UserBasicInfoDto(string Id , string ProfileId , string DisplayName , string ImageUrl = "") {
    public static UserBasicInfoDto New(string id , string profileId , string displayName , string imageUrl = "")
        => new(id , profileId , displayName , imageUrl);
}