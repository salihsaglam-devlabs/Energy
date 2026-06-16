using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.DocumentVersion.Services;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

namespace Energy.Infrastructure.Modules.Documents.DocumentVersion.Services;

/// <summary>DocumentVersion CRUD servisi (projection, pagination, soft-delete).</summary>
public class DocumentVersionService : IDocumentVersionService
{
    private readonly EnergyDbContext _db;

    public DocumentVersionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DocumentVersionListResponse>>> GetListAsync(GetDocumentVersionListRequest request, CancellationToken ct = default)
    {
        var query = _db.DocumentVersions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DocumentVersionListResponse
            {
                Id = e.Id,
                DocumentId = e.DocumentId,
                VersionNo = e.VersionNo,
                FileName = e.FileName,
                FilePath = e.FilePath,
                FileSize = e.FileSize,
                ContentType = e.ContentType,
                UploadedAt = e.UploadedAt,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DocumentVersionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DocumentVersionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DocumentVersionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DocumentVersions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DocumentVersionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                DocumentId = e.DocumentId,
                VersionNo = e.VersionNo,
                FileName = e.FileName,
                FilePath = e.FilePath,
                FileSize = e.FileSize,
                ContentType = e.ContentType,
                UploadedAt = e.UploadedAt
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DocumentVersionDetailResponse>.Failure("NotFound")
            : BaseResponse<DocumentVersionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDocumentVersionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Documents.DocumentVersion
        {
            Id = Guid.NewGuid(),
            DocumentId = request.DocumentId,
            VersionNo = request.VersionNo,
            FileName = request.FileName,
            FilePath = request.FilePath,
            FileSize = request.FileSize,
            ContentType = request.ContentType,
            UploadedAt = request.UploadedAt,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DocumentVersions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentVersionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DocumentVersions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DocumentId = request.DocumentId;
            entity.VersionNo = request.VersionNo;
            entity.FileName = request.FileName;
            entity.FilePath = request.FilePath;
            entity.FileSize = request.FileSize;
            entity.ContentType = request.ContentType;
            entity.UploadedAt = request.UploadedAt;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DocumentVersions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
