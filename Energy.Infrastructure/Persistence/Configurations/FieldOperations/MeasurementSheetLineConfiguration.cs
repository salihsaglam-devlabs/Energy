using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.FieldOperations;

/// <summary>MeasurementSheetLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MeasurementSheetLineConfiguration : IEntityTypeConfiguration<MeasurementSheetLine>
{
    public void Configure(EntityTypeBuilder<MeasurementSheetLine> e)
    {
        e.ToTable("MeasurementSheetLines");
        e.HasOne<MeasurementSheet>().WithMany().HasForeignKey(x => x.MeasurementSheetId).OnDelete(DeleteBehavior.Cascade);
    }
}
