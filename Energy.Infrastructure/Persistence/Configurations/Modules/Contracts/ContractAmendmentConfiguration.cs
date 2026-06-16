using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Contracts;

/// <summary>ContractAmendment EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ContractAmendmentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Contracts.ContractAmendment>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Contracts.ContractAmendment> builder)
    {
        builder.ToTable("ContractAmendments");
        builder.HasKey(e => e.Id);
    }
}
