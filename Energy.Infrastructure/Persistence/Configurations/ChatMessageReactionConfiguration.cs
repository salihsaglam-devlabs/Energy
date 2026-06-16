using Energy.Domain.Chat;
using Energy.Domain.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

/// <summary>Sohbet mesajı tepkileri (emoji) için EF Core eşleme yapılandırması.</summary>
public sealed class ChatMessageReactionConfiguration : IEntityTypeConfiguration<ChatMessageReaction>
{
    /// <summary>Tablo, anahtar, kısıtlar ve ilişkileri yapılandırır.</summary>
    public void Configure(EntityTypeBuilder<ChatMessageReaction> builder)
    {
        builder.ToTable("ChatMessageReactions");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Emoji).IsRequired().HasMaxLength(16);

        // (mesaj, kullanıcı) ikilisi başına tek tepki satırı.
        builder.HasIndex(r => new { r.MessageId, r.UserId }).IsUnique();

        builder.HasOne<ChatMessage>().WithMany().HasForeignKey(r => r.MessageId).OnDelete(DeleteBehavior.Cascade);
        // Tepkiyi veren kullanıcı. Geçmiş kaydın bozulmaması için Restrict.
        builder.HasOne<User>().WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}

