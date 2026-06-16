using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.ProgressPayments;

/// <summary>ProgressPaymentLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ProgressPaymentLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentLine> builder)
    {
        builder.ToTable("ProgressPaymentLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.ProgressPayments.ProgressPayment>().WithMany().HasForeignKey(e => e.ProgressPaymentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Contracts.ContractLine>().WithMany().HasForeignKey(e => e.ContractLineId).OnDelete(DeleteBehavior.Restrict);
    }
}
