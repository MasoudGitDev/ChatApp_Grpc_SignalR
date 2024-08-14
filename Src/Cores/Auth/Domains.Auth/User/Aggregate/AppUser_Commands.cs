using Domains.Auth.User.ValueObjects;
using Shared.Server.Dtos;

namespace Domains.Auth.User.Aggregate;
public partial class AppUser {
    public static AppUser Create(RegisterDto model) {
        return new() {
            Id = model.Id ,
            UserName = model.UserName ,
            DisplayName = model.DisplayName ,
            ProfileId = model.ProfileId ,
            Email = model.Email ,
            EmailConfirmed = false ,
            CreatedAt = DateTime.UtcNow ,
        };
    }
    public static AppUser InvalidUser => new() {
        Id = Guid.Empty ,
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
