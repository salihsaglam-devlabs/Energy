using Energy.Domain.Modules.Budget;
using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.Finance;
using Energy.Domain.Modules.ProgressPayments;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

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
