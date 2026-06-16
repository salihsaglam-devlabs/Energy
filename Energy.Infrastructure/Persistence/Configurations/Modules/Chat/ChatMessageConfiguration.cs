using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Chat;

/// <summary>ChatMessage EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ChatMessageConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Chat.ChatMessage>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Chat.ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.SenderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.RecipientId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Chat.ChatGroup>().WithMany().HasForeignKey(e => e.GroupId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Chat.ChatMessage>().WithMany().HasForeignKey(e => e.ReplyToMessageId).OnDelete(DeleteBehavior.Restrict);
    }
}
