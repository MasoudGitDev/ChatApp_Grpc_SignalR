using Domains.Chat.ChatAggregate;
using Domains.Chat.MessageAggregate;
using Domains.Chat.MessageAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EFCore.Configs.Chat;
internal class ChatConfigs :
    IEntityTypeConfiguration<ChatItem>,
    IEntityTypeConfiguration<ChatMessage> {
    public void Configure(EntityTypeBuilder<ChatItem> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.RequesterId , x.ReceiverId }).IsUnique();
        builder.HasMany(x => x.Messages).WithOne(x => x.Chat).IsRequired()
            .HasForeignKey(x => x.ChatId).IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Receiver).WithMany().IsRequired()
          .HasForeignKey(x => x.ReceiverId).IsRequired()
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Requester).WithMany().IsRequired()
        .HasForeignKey(x => x.RequesterId).IsRequired()
        .OnDelete(DeleteBehavior.NoAction);
    }

    public void Configure(EntityTypeBuilder<ChatMessage> builder) {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.FileUrl).HasConversion(model => model.Value , value => FileUrl.Create(value));
        builder.Property(x => x.ChatId);

        builder.HasOne(x => x.Sender).WithMany().IsRequired()
           .HasForeignKey(x => x.SenderId).IsRequired()
           .OnDelete(DeleteBehavior.NoAction);
    }
}
