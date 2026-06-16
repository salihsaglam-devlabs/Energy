using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.DocumentFolder.Services;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;

namespace Energy.Infrastructure.Modules.Documents.DocumentFolder.Services;

/// <summary>DocumentFolder CRUD servisi (projection, pagination, soft-delete).</summary>
public class DocumentFolderService : IDocumentFolderService
{
    private readonly AppDbContext _db;

    public DocumentFolderService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DocumentFolderListResponse>>> GetListAsync(GetDocumentFolderListRequest request, CancellationToken ct = default)
    {
        var query = _db.DocumentFolders.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DocumentFolderListResponse
            {
                Id = e.Id,
                ParentFolderId = e.ParentFolderId,
                Name = e.Name,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DocumentFolderListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DocumentFolderListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DocumentFolderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DocumentFolders.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DocumentFolderDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ParentFolderId = e.ParentFolderId,
                Name = e.Name
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DocumentFolderDetailResponse>.Failure("NotFound")
            : BaseResponse<DocumentFolderDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDocumentFolderRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Documents.DocumentFolder
        {
            Id = Guid.NewGuid(),
            ParentFolderId = request.ParentFolderId,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DocumentFolders.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentFolderRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DocumentFolders.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ParentFolderId = request.ParentFolderId;
            entity.Name = request.Name;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DocumentFolders.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
