using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.Document.Services;
using Energy.Shared.Models.V1.Documents.Document.Requests;
using Energy.Shared.Models.V1.Documents.Document.Responses;

namespace Energy.Infrastructure.Modules.Documents.Document.Services;

/// <summary>Document CRUD servisi (projection, pagination, soft-delete).</summary>
public class DocumentService : IDocumentService
{
    private readonly AppDbContext _db;

    public DocumentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DocumentListResponse>>> GetListAsync(GetDocumentListRequest request, CancellationToken ct = default)
    {
        var query = _db.Documents.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DocumentListResponse
            {
                Id = e.Id,
                DocumentFolderId = e.DocumentFolderId,
                Name = e.Name,
                Description = e.Description,
                Status = e.Status,
                CurrentVersionNo = e.CurrentVersionNo,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DocumentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DocumentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DocumentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Documents.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DocumentDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                DocumentFolderId = e.DocumentFolderId,
                Name = e.Name,
                Description = e.Description,
                Status = e.Status,
                CurrentVersionNo = e.CurrentVersionNo
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DocumentDetailResponse>.Failure("NotFound")
            : BaseResponse<DocumentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDocumentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Documents.Document
        {
            Id = Guid.NewGuid(),
            DocumentFolderId = request.DocumentFolderId,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            CurrentVersionNo = request.CurrentVersionNo,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Documents.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Documents.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DocumentFolderId = request.DocumentFolderId;
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Status = request.Status;
            entity.CurrentVersionNo = request.CurrentVersionNo;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Documents.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
