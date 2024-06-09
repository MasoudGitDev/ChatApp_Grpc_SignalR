using Domains.Auth.User.Aggregate;
using Domains.Chats.Contacts.Aggregate;
using Domains.Chats.Item.Aggregate;
using Domains.Chats.Message.Aggregate;
using Domains.Chats.Message.ValueObjects;
using Domains.Chats.Requests.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EFCore.Configs.Chat;
internal class ChatConfigs :
    IEntityTypeConfiguration<ChatItem>,
    IEntityTypeConfiguration<ChatMessage> ,
    IEntityTypeConfiguration<Contact> ,
    IEntityTypeConfiguration<ChatRequest>{
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

    public void Configure(EntityTypeBuilder<ChatRequest> builder) {
        builder.ToTable("ChatRequests");
        builder.HasKey(x => new { x.RequesterId , x.ReceiverId });
        builder.HasOne<AppUser>().WithMany().HasForeignKey(x=> new { x.RequesterId })
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Contact> builder) {
        builder.ToTable("Contacts");
        builder.HasKey(x => new { x.RequesterId , x.ReceiverId });
        builder.HasOne<AppUser>().WithMany().HasForeignKey(x => new { x.RequesterId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}
