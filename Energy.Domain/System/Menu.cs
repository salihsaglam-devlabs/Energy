using Energy.Domain.Common;

namespace Energy.Domain.System;

public class Menu : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public int Order { get; set; }

    public Guid? ParentId { get; set; }
}
