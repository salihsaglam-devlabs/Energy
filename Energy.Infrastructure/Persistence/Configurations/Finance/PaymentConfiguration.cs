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

/// <summary>Payment EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> e)
    {
        e.ToTable("Payments");
        e.HasIndex(x => x.PaymentNo).IsUnique();
        e.HasOne<BusinessPartner>().WithMany().HasForeignKey(x => x.PartnerId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<FinancialAccount>().WithMany().HasForeignKey(x => x.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
    }
}
