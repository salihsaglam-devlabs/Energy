using Energy.Application.Common.Storage;
using Energy.Application.Modules.Documents.Files.Services;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Modules.Documents.Files;

/// <summary>
/// Belge dosya/versiyon yönetimi servisi. Dosyayı saklama soyutlamasına yazar,
/// ardından DocumentVersion kaydını oluşturur ve Document.CurrentVersionNo değerini
/// artırır (tek SaveChanges = atomik). DB hata alırsa yazılan dosya geri alınır.
/// </summary>
public sealed class DocumentFileService : IDocumentFileService
{
    private readonly EnergyDbContext _db;
    private readonly IFileStorage _storage;

    public DocumentFileService(EnergyDbContext db, IFileStorage storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<BaseResponse<DocumentVersionFileResponse>> UploadNewVersionAsync(
        Guid documentId, Stream content, string fileName, string? contentType, long size, CancellationToken ct = default)
    {
        var document = await _db.Documents.FirstOrDefaultAsync(d => d.Id == documentId, ct);
        if (document is null)
        {
            return BaseResponse<DocumentVersionFileResponse>.Failure("NotFound");
        }

        var nextVersionNo = document.CurrentVersionNo + 1;
        var relativePath = await _storage.SaveAsync(content, fileName, ct);

        try
        {
            var version = new global::Energy.Domain.Modules.Documents.DocumentVersion
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                VersionNo = nextVersionNo,
                FileName = fileName,
                FilePath = relativePath,
                FileSize = size,
                ContentType = contentType,
                UploadedAt = DateTime.UtcNow,
            };
            _db.DocumentVersions.Add(version);
            document.CurrentVersionNo = nextVersionNo;
            await _db.SaveChangesAsync(ct);

            return BaseResponse<DocumentVersionFileResponse>.Success(Map(version), "Uploaded");
        }
        catch
        {
            await _storage.DeleteAsync(relativePath, CancellationToken.None);
            throw;
        }
    }

    public async Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> GetVersionsAsync(Guid documentId, CancellationToken ct = default)
    {
        var items = await _db.DocumentVersions.AsNoTracking()
            .Where(v => v.DocumentId == documentId)
            .OrderByDescending(v => v.VersionNo)
            .Select(v => new DocumentVersionFileResponse
            {
                Id = v.Id,
                DocumentId = v.DocumentId,
                VersionNo = v.VersionNo,
                FileName = v.FileName,
                FileSize = v.FileSize,
                ContentType = v.ContentType,
                UploadedAt = v.UploadedAt,
            })
            .ToListAsync(ct);

        return BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>.Success(items);
    }

    public async Task<DocumentDownload?> GetVersionContentAsync(Guid versionId, CancellationToken ct = default)
    {
        var version = await _db.DocumentVersions.AsNoTracking().FirstOrDefaultAsync(v => v.Id == versionId, ct);
        if (version is null)
        {
            return null;
        }

        var stream = await _storage.OpenAsync(version.FilePath, ct);
        if (stream is null)
        {
            return null;
        }

        return new DocumentDownload(stream, version.FileName, version.ContentType ?? "application/octet-stream");
    }

    private static DocumentVersionFileResponse Map(global::Energy.Domain.Modules.Documents.DocumentVersion v) => new()
    {
        Id = v.Id,
        DocumentId = v.DocumentId,
        VersionNo = v.VersionNo,
        FileName = v.FileName,
        FileSize = v.FileSize,
        ContentType = v.ContentType,
        UploadedAt = v.UploadedAt,
    };
}
