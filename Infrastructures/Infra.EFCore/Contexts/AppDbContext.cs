using Domain.Auth.RoleAggregate;
using Domain.Auth.UserAggregate;
using Domains.Chat.ChatAggregate;
using Domains.Chat.MessageAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.EFCore.Contexts;
internal class AppDbContext : IdentityDbContext<AppUser , AppRole , Guid> {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //  builder.Ignore<IdentityUserRole<Guid>>();
        builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId , p.RoleId });
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<ChatItem> ChatItems { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }


}
