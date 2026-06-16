using Energy.Domain.Modules.Chat;
using Energy.Domain.Modules.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class ChatGroupConfiguration : IEntityTypeConfiguration<ChatGroup>
{
    public void Configure(EntityTypeBuilder<ChatGroup> builder)
    {
        builder.ToTable("ChatGroups");
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name).IsRequired().HasMaxLength(150);
        builder.HasIndex(g => g.OwnerId);

        // Grup sahibi kullanıcısı. Geçmiş grubun bozulmaması için Restrict.
        builder.HasOne<User>().WithMany().HasForeignKey(g => g.OwnerId).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}

