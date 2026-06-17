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
