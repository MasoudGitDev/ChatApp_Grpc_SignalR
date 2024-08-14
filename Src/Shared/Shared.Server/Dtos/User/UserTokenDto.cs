namespace Shared.Server.Dtos.User;
public record UserTokenDto(Guid Id , string UserName = "" , string DisplayName = "") {
    public static UserTokenDto New(Guid userId,string userName = "", string displayName = "")
        => new(userId,userName,displayName);
}
