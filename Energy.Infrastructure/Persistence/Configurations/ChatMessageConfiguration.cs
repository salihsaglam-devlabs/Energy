using Energy.Domain.Chat;
using Energy.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Text).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.IsRead).HasDefaultValue(false);

        builder.Property(m => m.AttachmentFileName).HasMaxLength(260);
        builder.Property(m => m.AttachmentContentType).HasMaxLength(150);

        // Restrict kullanılıyor çünkü SenderId ve RecipientId aynı Users tablosuna işaret ediyor.
        // Her ikisi de Cascade olursa SQL Server "çoklu cascade yolu" hatası verir (Error 1785).
        builder.HasOne<User>().WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<User>().WithMany().HasForeignKey(m => m.RecipientId).OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => new { m.SenderId, m.RecipientId });
        builder.HasIndex(m => new { m.RecipientId, m.IsRead });

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}

