using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Chat;

/// <summary>ChatGroup EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ChatGroupConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Chat.ChatGroup>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Chat.ChatGroup> builder)
    {
        builder.ToTable("ChatGroups");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.OwnerId).OnDelete(DeleteBehavior.Restrict);
    }
}
