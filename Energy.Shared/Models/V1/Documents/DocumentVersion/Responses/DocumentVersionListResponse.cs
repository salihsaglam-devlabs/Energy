namespace Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

/// <summary>DocumentVersion liste satırı.</summary>
public class DocumentVersionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
