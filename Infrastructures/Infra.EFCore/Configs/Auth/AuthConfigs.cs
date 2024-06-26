using Domains.Auth.User.Aggregate;
using Domains.Auth.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EFCore.Configs.Auth;
//internal class AuthConfigs {}

internal class UserEFConfigs : IEntityTypeConfiguration<AppUser> {
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder.HasIndex(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x=> x.ProfileId).IsRequired().HasConversion(x=> x.Value , y=>ProfileId.Create(y));
    }
}
