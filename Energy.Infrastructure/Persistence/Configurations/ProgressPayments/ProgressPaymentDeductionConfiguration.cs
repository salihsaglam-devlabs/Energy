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

namespace Energy.Infrastructure.Persistence.Configurations.ProgressPayments;

/// <summary>ProgressPaymentDeduction EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProgressPaymentDeductionConfiguration : IEntityTypeConfiguration<ProgressPaymentDeduction>
{
    public void Configure(EntityTypeBuilder<ProgressPaymentDeduction> e)
    {
        e.ToTable("ProgressPaymentDeductions");
        e.HasOne<ProgressPayment>().WithMany().HasForeignKey(x => x.ProgressPaymentId).OnDelete(DeleteBehavior.Cascade);
    }
}
