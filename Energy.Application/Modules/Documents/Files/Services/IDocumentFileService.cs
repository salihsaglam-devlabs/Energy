using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;

namespace Energy.Application.Modules.Documents.Files.Services;

/// <summary>İndirilebilir belge içeriği (stream + meta).</summary>
public sealed record DocumentDownload(Stream Content, string FileName, string ContentType);

/// <summary>Belge dosya/versiyon yönetimi sözleşmesi (transaction-güvenli, storage soyutlaması üzerinden).</summary>
public interface IDocumentFileService
{
    /// <summary>Belgeye yeni bir versiyon yükler; CurrentVersionNo artırılır (tek işlem).</summary>
    Task<BaseResponse<DocumentVersionFileResponse>> UploadNewVersionAsync(
        Guid documentId, Stream content, string fileName, string? contentType, long size, CancellationToken ct = default);

    /// <summary>Belgenin versiyon geçmişini (yeniden eskiye) döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> GetVersionsAsync(Guid documentId, CancellationToken ct = default);

    /// <summary>Bir versiyonun dosya içeriğini indirir; yoksa null.</summary>
    Task<DocumentDownload?> GetVersionContentAsync(Guid versionId, CancellationToken ct = default);
}
