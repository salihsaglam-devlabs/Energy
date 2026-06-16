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

/// <summary>ProgressPayment EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ProgressPaymentConfiguration : IEntityTypeConfiguration<ProgressPayment>
{
    public void Configure(EntityTypeBuilder<ProgressPayment> e)
    {
        e.ToTable("ProgressPayments");
        e.HasIndex(x => x.ProgressPaymentNo).IsUnique();
        e.HasOne<Contract>().WithMany().HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
    }
}
