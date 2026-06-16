namespace Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

/// <summary>DocumentVersion detay görünümü.</summary>
public class DocumentVersionDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
