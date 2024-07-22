namespace Shared.Server.Dtos.User;

public record OnlineUserDto(string ProfileId , string DisplayName , string ImageUrl = "" , bool IsOnline = false) {
    public static OnlineUserDto New(string profileId , string displayName , string imageUrl = "" , bool isOnline = false)
        => new( profileId,displayName,imageUrl, isOnline);
}