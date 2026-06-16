using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Contracts;

/// <summary>ContractLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ContractLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Contracts.ContractLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Contracts.ContractLine> builder)
    {
        builder.ToTable("ContractLines");
        builder.HasKey(e => e.Id);
    }
}
