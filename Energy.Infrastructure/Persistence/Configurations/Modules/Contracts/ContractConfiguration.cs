using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Contracts;

/// <summary>Contract EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ContractConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Contracts.Contract>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Contracts.Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Currency>().WithMany().HasForeignKey(e => e.CurrencyId).OnDelete(DeleteBehavior.Restrict);
    }
}
