using Energy.Domain.Modules.Chat;
using Energy.Domain.Modules.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

/// <summary>Sohbet grubu üyelikleri için EF Core eşleme yapılandırması.</summary>
public sealed class ChatGroupMemberConfiguration : IEntityTypeConfiguration<ChatGroupMember>
{
    /// <summary>Tablo, anahtar, kısıtlar ve ilişkileri yapılandırır.</summary>
    public void Configure(EntityTypeBuilder<ChatGroupMember> builder)
    {
        builder.ToTable("ChatGroupMembers");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Status).HasConversion<int>();

        // (grup, kullanıcı) ikilisi başına tek üyelik satırı.
        builder.HasIndex(m => new { m.GroupId, m.UserId }).IsUnique();
        builder.HasIndex(m => new { m.UserId, m.Status });

        builder.HasOne<ChatGroup>().WithMany().HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Cascade);
        // Üye kullanıcısı. Geçmiş üyelik kaydının bozulmaması için Restrict.
        builder.HasOne<User>().WithMany().HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}

