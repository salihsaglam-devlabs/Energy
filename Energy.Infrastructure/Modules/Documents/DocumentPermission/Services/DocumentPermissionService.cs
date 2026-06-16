using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.DocumentPermission.Services;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Requests;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

namespace Energy.Infrastructure.Modules.Documents.DocumentPermission.Services;

/// <summary>DocumentPermission CRUD servisi (projection, pagination, soft-delete).</summary>
public class DocumentPermissionService : IDocumentPermissionService
{
    private readonly AppDbContext _db;

    public DocumentPermissionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>> GetListAsync(GetDocumentPermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.DocumentPermissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DocumentPermissionListResponse
            {
                Id = e.Id,
                DocumentId = e.DocumentId,
                UserId = e.UserId,
                RoleId = e.RoleId,
                AccessType = e.AccessType,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DocumentPermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DocumentPermissionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DocumentPermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DocumentPermissions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DocumentPermissionDetailResponse
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
                UserId = e.UserId,
                RoleId = e.RoleId,
                AccessType = e.AccessType
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DocumentPermissionDetailResponse>.Failure("NotFound")
            : BaseResponse<DocumentPermissionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDocumentPermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Documents.DocumentPermission
        {
            Id = Guid.NewGuid(),
            DocumentId = request.DocumentId,
            UserId = request.UserId,
            RoleId = request.RoleId,
            AccessType = request.AccessType,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DocumentPermissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentPermissionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DocumentPermissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DocumentId = request.DocumentId;
            entity.UserId = request.UserId;
            entity.RoleId = request.RoleId;
            entity.AccessType = request.AccessType;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DocumentPermissions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
