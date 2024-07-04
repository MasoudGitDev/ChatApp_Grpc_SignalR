using Domains.Auth.User.ValueObjects;
using Shared.Server.Dtos;

namespace Domains.Auth.User.Aggregate;
public partial class AppUser {
    public static AppUser Create(RegisterDto model , Guid? userId = null) {
        return new AppUser() {
            Id = userId ?? Guid.NewGuid() ,
            UserName = model.UserName ,
            Email = model.Email ,
            EmailConfirmed = false ,
            CreatedAt = DateTime.UtcNow ,
        };
    }
    public static AppUser Empty => new() { Id = Guid.Empty ,
        Email = "<invalid-email>" ,
        UserName = "<invalid-userName>" ,
        ProfileId = "<invalid-profileId>"
    };

    public void Update(ProfileId profileId) {
        if(UserName == profileId || Email == profileId) {
            throw new Exception("Your Email or UserName can not be used as profileId!");
        }
        ProfileId = profileId;
    }

}
