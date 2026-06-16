using Energy.Domain.Common;

namespace Energy.Domain.Modules.Documents;

/// <summary>Belge versiyonu (fiziksel dosya tek kez saklanır, yeni yükleme yeni versiyondur).</summary>
public class DocumentVersion : AuditableEntity
{
    public Guid DocumentId { get; set; }
    public int VersionNo { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? ContentType { get; set; }
    public DateTime UploadedAt { get; set; }
}
