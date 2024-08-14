
using Domains.Auth.Online.Aggregate;
using Domains.Auth.User.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Shared.Server.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Auth.User.Aggregate;

[Table("AppUsers")]
public partial class AppUser : IdentityUser<Guid> {

    public DateTime CreatedAt { get; set; }
    public ProfileId ProfileId { get; private set; } = Guid.NewGuid().ToString().Replace("-" , "");
    public string DisplayName { get; set; } = "User";
    public string ImageUrl { get; set; } = String.Empty;

    //=============relations
    public OnlineUser OnlineUser { get; private set; } = null!;


}