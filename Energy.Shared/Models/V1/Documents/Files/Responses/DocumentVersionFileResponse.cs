namespace Energy.Shared.Models.V1.Documents.Files.Responses;

/// <summary>Bir belge versiyonunun dosya meta verisi (salt-okunur).</summary>
public sealed class DocumentVersionFileResponse
{
    /// <summary>Versiyon kaydı kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Bağlı belge kimliği.</summary>
    public Guid DocumentId { get; set; }

    /// <summary>Sıra numarası (1, 2, 3, ...).</summary>
    public int VersionNo { get; set; }

    /// <summary>Yüklenen dosya adı.</summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>Dosya boyutu (bayt).</summary>
    public long FileSize { get; set; }

    /// <summary>MIME içerik türü.</summary>
    public string? ContentType { get; set; }

    /// <summary>Yüklenme zamanı.</summary>
    public DateTime UploadedAt { get; set; }
}
