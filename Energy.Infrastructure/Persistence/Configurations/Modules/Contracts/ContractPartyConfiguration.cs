using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Contracts;

/// <summary>ContractParty EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ContractPartyConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Contracts.ContractParty>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Contracts.ContractParty> builder)
    {
        builder.ToTable("ContractParties");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Contracts.Contract>().WithMany().HasForeignKey(e => e.ContractId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner>().WithMany().HasForeignKey(e => e.BusinessPartnerId).OnDelete(DeleteBehavior.Restrict);
    }
}
