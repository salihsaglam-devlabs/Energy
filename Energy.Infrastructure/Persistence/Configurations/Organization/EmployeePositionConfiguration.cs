using Energy.Domain.BusinessPartners;
using Energy.Domain.Core;
using Energy.Domain.IAM;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Organization;

/// <summary>EmployeePosition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class EmployeePositionConfiguration : IEntityTypeConfiguration<EmployeePosition>
{
    public void Configure(EntityTypeBuilder<EmployeePosition> e)
    {
        e.ToTable("EmployeePositions"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}
