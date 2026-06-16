using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Chat;

/// <summary>ChatGroupMember EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ChatGroupMemberConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Chat.ChatGroupMember>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Chat.ChatGroupMember> builder)
    {
        builder.ToTable("ChatGroupMembers");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Chat.ChatGroup>().WithMany().HasForeignKey(e => e.GroupId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
