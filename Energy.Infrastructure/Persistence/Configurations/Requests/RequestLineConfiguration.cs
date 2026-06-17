using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Procurement;
using Energy.Domain.Projects;
using Energy.Domain.Requests;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Requests;

/// <summary>RequestLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class RequestLineConfiguration : IEntityTypeConfiguration<RequestLine>
{
    public void Configure(EntityTypeBuilder<RequestLine> e)
    {
        e.ToTable("RequestLines");
        e.HasOne<Request>().WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
    }
}
