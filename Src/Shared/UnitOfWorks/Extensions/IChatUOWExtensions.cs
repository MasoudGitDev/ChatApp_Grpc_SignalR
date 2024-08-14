using Domains.Auth.User.Aggregate;
using Shared.Server.Dtos.User;
using UnitOfWorks.Abstractions;

namespace UnitOfWorks.Extensions;
public static class IChatUOWExtensions {
    public static IChatUOW HasValue(this IChatUOW? chatUOW) {
        ArgumentNullException.ThrowIfNull(chatUOW);
        return chatUOW;
    }

    public static UserBasicInfoDto ToBasicInfo(this AppUser appUser)
        => new(appUser.ProfileId , appUser.DisplayName , appUser.ImageUrl);
}
