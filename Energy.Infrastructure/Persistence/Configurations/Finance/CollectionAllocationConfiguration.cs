using Energy.Domain.Budget;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.Finance;
using Energy.Domain.ProgressPayments;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Finance;

/// <summary>CollectionAllocation EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class CollectionAllocationConfiguration : IEntityTypeConfiguration<CollectionAllocation>
{
    public void Configure(EntityTypeBuilder<CollectionAllocation> e)
    {
        e.ToTable("CollectionAllocations");
        e.HasOne<Collection>().WithMany().HasForeignKey(x => x.CollectionId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Receivable>().WithMany().HasForeignKey(x => x.ReceivableId).OnDelete(DeleteBehavior.Restrict);
    }
}
