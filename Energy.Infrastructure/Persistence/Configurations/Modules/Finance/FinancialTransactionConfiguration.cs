using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>FinancialTransaction EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class FinancialTransactionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.FinancialTransaction> builder)
    {
        builder.ToTable("FinancialTransactions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner>().WithMany().HasForeignKey(e => e.PartnerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Currency>().WithMany().HasForeignKey(e => e.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
