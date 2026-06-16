using Energy.Domain.Common;

namespace Energy.Domain.Requests;

/// <summary>Talep türü.</summary>
public class RequestType : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Material, Service, Equipment, Personnel, Other.</summary>
    public string Category { get; set; } = "Material";
    public bool IsActive { get; set; } = true;
}
