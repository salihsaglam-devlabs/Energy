using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>
/// Belge versiyonları
/// </summary>
public class DocumentVersion : AuditableEntity
{
    /// <summary>Documents referansı</summary>
    public Guid DocumentId { get; set; }

    /// <summary>VersionNo</summary>
    public int VersionNo { get; set; }

    /// <summary>FileName</summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>FilePath</summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>FileSize</summary>
    public long FileSize { get; set; }

    /// <summary>ContentType</summary>
    public string? ContentType { get; set; }

    /// <summary>UploadedAt</summary>
    public DateTime UploadedAt { get; set; }
}
