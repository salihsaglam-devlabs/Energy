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

/// <summary>PaymentAllocation EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class PaymentAllocationConfiguration : IEntityTypeConfiguration<PaymentAllocation>
{
    public void Configure(EntityTypeBuilder<PaymentAllocation> e)
    {
        e.ToTable("PaymentAllocations");
        e.HasOne<Payment>().WithMany().HasForeignKey(x => x.PaymentId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Payable>().WithMany().HasForeignKey(x => x.PayableId).OnDelete(DeleteBehavior.Restrict);
    }
}
