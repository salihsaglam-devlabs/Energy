using Energy.Domain.Common;

namespace Energy.Domain.Identity;

public class Role : BaseEntity
{
    public string Description { get; set; } = string.Empty;

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }
}
