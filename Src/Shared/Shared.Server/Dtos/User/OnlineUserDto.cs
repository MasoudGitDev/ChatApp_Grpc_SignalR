namespace Shared.Server.Dtos.User;

public record OnlineUserDto(UserBasicInfoDto BasicInfo , bool IsOnline , bool IsInContacts = false) {
    public static OnlineUserDto New(UserBasicInfoDto basicInfo , bool isOnline , bool isInContacts = false)
        => new(basicInfo, isOnline ,isInContacts);
}