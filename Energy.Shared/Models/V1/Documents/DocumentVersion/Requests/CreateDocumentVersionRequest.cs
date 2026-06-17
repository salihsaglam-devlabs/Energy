namespace Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;

/// <summary>DocumentVersion oluşturma isteği.</summary>
public class CreateDocumentVersionRequest
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
