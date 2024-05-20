
using Microsoft.AspNetCore.Identity;
using Shared.Server.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Auth.UserAggregate;

[Table("AppUsers")]
public partial class AppUser :IdentityUser<Guid> {

    public DateTime CreatedAt { get; set; }

}


public partial class AppUser {
    public static AppUser Create(RegisterDto model , Guid? userId = null) {
        return new AppUser() { 
            Id = userId ?? Guid.NewGuid(),
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = false ,   
            CreatedAt = DateTime.UtcNow,
        };
    }
}
