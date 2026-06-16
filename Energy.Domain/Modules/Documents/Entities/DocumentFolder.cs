using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>
/// Belge klasörleri
/// </summary>
public class DocumentFolder : AuditableEntity
{
    /// <summary>ParentFolderId</summary>
    public Guid? ParentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;
}
