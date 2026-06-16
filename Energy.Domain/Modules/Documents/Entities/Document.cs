using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>
/// Belge kayıtları
/// </summary>
public class Document : AuditableEntity
{
    /// <summary>DocumentFolderId</summary>
    public Guid? DocumentFolderId { get; set; }

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>CurrentVersionNo</summary>
    public int CurrentVersionNo { get; set; }
}
