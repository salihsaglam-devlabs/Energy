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

namespace Energy.Infrastructure.Persistence.Configurations.Modules.ProgressPayments;

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
