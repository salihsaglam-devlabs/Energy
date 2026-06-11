using Energy.Domain.Common;

namespace Energy.Domain.System;

public class AccessRule : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Scope { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string HttpMethod { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;
}

