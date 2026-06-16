using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>ApiEndpoint EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ApiEndpointConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.ApiEndpoint>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.ApiEndpoint> builder)
    {
        builder.ToTable("ApiEndpoints");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Permission>().WithMany().HasForeignKey(e => e.RequiredPermissionCode).HasPrincipalKey("Code").OnDelete(DeleteBehavior.SetNull);
    }
}
