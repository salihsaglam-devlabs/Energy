using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Requests;

/// <summary>Request EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class RequestConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Requests.Request>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Requests.Request> builder)
    {
        builder.ToTable("Requests");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Requests.RequestType>().WithMany().HasForeignKey(e => e.RequestTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.RequestedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
