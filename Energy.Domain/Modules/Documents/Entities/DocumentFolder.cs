using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>Belge klasörü (hiyerarşik).</summary>
public class DocumentFolder : AuditableEntity
{
    public Guid? ParentFolderId { get; set; }
    public string Name { get; set; } = string.Empty;
}
