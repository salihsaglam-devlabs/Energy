using Energy.Domain.Common;

namespace Energy.Domain.Identity;

public class Permission : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
