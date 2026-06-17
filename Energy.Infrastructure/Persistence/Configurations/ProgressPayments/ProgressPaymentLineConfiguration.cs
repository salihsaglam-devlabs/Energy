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

/// <summary>ProgressPaymentLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProgressPaymentLineConfiguration : IEntityTypeConfiguration<ProgressPaymentLine>
{
    public void Configure(EntityTypeBuilder<ProgressPaymentLine> e)
    {
        e.ToTable("ProgressPaymentLines");
        e.HasOne<ProgressPayment>().WithMany().HasForeignKey(x => x.ProgressPaymentId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<ContractLine>().WithMany().HasForeignKey(x => x.ContractLineId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<MeasurementSheetLine>().WithMany().HasForeignKey(x => x.MeasurementSheetLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
