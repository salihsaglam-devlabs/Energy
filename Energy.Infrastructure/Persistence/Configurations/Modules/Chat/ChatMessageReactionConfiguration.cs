using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Chat;

/// <summary>ChatMessageReaction EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ChatMessageReactionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Chat.ChatMessageReaction>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Chat.ChatMessageReaction> builder)
    {
        builder.ToTable("ChatMessageReactions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Chat.ChatMessage>().WithMany().HasForeignKey(e => e.MessageId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
