using Energy.Domain.Core;
using Energy.Domain.IAM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Core;

/// <summary>Branch EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> e)
    {
        e.ToTable("Branches");
        e.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
    }
}
