using System.Net.Http.Headers;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Documents.Files;

/// <summary>Belge dosya/versiyon API istemci sözleşmesi.</summary>
public interface IDocumentFilesApiClient
{
    Task<BaseResponse<DocumentVersionFileResponse>> UploadAsync(
        Guid documentId, Stream content, string fileName, string? contentType, CancellationToken ct = default);

    Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> GetVersionsAsync(Guid documentId, CancellationToken ct = default);

    Task<(byte[] Content, string ContentType, int StatusCode)> DownloadAsync(Guid versionId, CancellationToken ct = default);
}

/// <summary>Belge dosya/versiyon API istemcisi (multipart upload + ham indirme).</summary>
public sealed class DocumentFilesApiClient : ApiClientBase, IDocumentFilesApiClient
{
    private const string Base = "api/v1/documents/files";

    public DocumentFilesApiClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<BaseResponse<DocumentVersionFileResponse>> UploadAsync(
        Guid documentId, Stream content, string fileName, string? contentType, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent();
        var fileContent = new StreamContent(content);
        if (!string.IsNullOrWhiteSpace(contentType))
        {
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        }
        form.Add(new StringContent(documentId.ToString()), "documentId");
        form.Add(fileContent, "file", fileName);
        return await PostMultipartAsync<BaseResponse<DocumentVersionFileResponse>>($"{Base}/upload", form, ct);
    }

    public Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> GetVersionsAsync(Guid documentId, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>($"{Base}/versions/{documentId}", ct);

    public Task<(byte[] Content, string ContentType, int StatusCode)> DownloadAsync(Guid versionId, CancellationToken ct = default)
        => GetRawAsync($"{Base}/download/{versionId}", ct);
}

