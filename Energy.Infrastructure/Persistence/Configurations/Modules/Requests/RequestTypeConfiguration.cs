using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Requests;

/// <summary>RequestType EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class RequestTypeConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Requests.RequestType>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Requests.RequestType> builder)
    {
        builder.ToTable("RequestTypes");
        builder.HasKey(e => e.Id);
    }
}
