using Energy.Domain.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class ChatMessageReactionConfiguration : IEntityTypeConfiguration<ChatMessageReaction>
{
    public void Configure(EntityTypeBuilder<ChatMessageReaction> builder)
    {
        builder.ToTable("ChatMessageReactions");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Emoji).IsRequired().HasMaxLength(16);

        // One reaction row per (message, user).
        builder.HasIndex(r => new { r.MessageId, r.UserId }).IsUnique();

        builder.HasOne<ChatMessage>().WithMany().HasForeignKey(r => r.MessageId).OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}

