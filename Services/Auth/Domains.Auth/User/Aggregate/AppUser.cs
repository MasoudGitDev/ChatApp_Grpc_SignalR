
using Microsoft.AspNetCore.Identity;
using Shared.Server.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Auth.User.Aggregate;

[Table("AppUsers")]
public partial class AppUser : IdentityUser<Guid> {

    public DateTime CreatedAt { get; set; }

}


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
    public static AppUser Empty => new() { Id = Guid.Empty , Email = "<invalid-email" , UserName = "<invalid-username>" };

}
