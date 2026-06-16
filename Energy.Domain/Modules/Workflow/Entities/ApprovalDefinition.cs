using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>
/// Onay akışı tanımları
/// </summary>
public class ApprovalDefinition : AuditableEntity
{
    /// <summary>Akış kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Akış adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>İlgili modül</summary>
    public string RelatedModule { get; set; } = string.Empty;

    /// <summary>İlgili nesne türü</summary>
    public string RelatedEntityType { get; set; } = string.Empty;

    /// <summary>Aktiflik</summary>
    public bool IsActive { get; set; }
}
