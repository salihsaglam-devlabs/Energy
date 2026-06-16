using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>Branch EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class BranchConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.Branch>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Core.Company>().WithMany().HasForeignKey(e => e.CompanyId).OnDelete(DeleteBehavior.Restrict);
    }
}
