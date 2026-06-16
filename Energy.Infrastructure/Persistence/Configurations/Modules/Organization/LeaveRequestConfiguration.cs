using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Organization;

/// <summary>LeaveRequest EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class LeaveRequestConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Organization.LeaveRequest>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Organization.LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests");
        builder.HasKey(e => e.Id);
    }
}
