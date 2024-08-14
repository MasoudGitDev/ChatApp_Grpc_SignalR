using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.SqlServerWithEF.Configs.Chat;
internal class ChatConfigs :
    IEntityTypeConfiguration<ChatItem>,
    IEntityTypeConfiguration<ChatMessage> {
    public void Configure(EntityTypeBuilder<ChatItem> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.RequesterId , x.ReceiverId }).IsUnique();

        builder.HasMany(x => x.Messages).WithOne(x => x.ChatItem).IsRequired()
            .HasForeignKey(x => x.ChatItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<ChatMessage> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FileUrl).HasConversion(model => model.Value , value => FileUrl.Create(value));

        builder.HasOne(x => x.ChatItem).WithMany(x => x.Messages).IsRequired()
          .HasForeignKey(x => x.ChatItemId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Sender).WithMany().IsRequired()
           .HasForeignKey(x => x.SenderId).IsRequired()
           .OnDelete(DeleteBehavior.NoAction);
    }
}
