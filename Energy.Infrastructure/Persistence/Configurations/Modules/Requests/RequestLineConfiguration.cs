using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Requests;

/// <summary>RequestLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class RequestLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Requests.RequestLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Requests.RequestLine> builder)
    {
        builder.ToTable("RequestLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Requests.Request>().WithMany().HasForeignKey(e => e.RequestId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Material>().WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.UnitOfMeasure>().WithMany().HasForeignKey(e => e.UnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
    }
}
