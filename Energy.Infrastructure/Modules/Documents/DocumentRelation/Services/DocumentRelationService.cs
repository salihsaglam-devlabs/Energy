using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.DocumentRelation.Services;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Requests;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;

namespace Energy.Infrastructure.Modules.Documents.DocumentRelation.Services;

/// <summary>DocumentRelation CRUD servisi (projection, pagination, soft-delete).</summary>
public class DocumentRelationService : IDocumentRelationService
{
    private readonly EnergyDbContext _db;

    public DocumentRelationService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DocumentRelationListResponse>>> GetListAsync(GetDocumentRelationListRequest request, CancellationToken ct = default)
    {
        var query = _db.DocumentRelations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DocumentRelationListResponse
            {
                Id = e.Id,
                DocumentId = e.DocumentId,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DocumentRelationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DocumentRelationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DocumentRelationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DocumentRelations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DocumentRelationDetailResponse
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
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DocumentRelationDetailResponse>.Failure("NotFound")
            : BaseResponse<DocumentRelationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDocumentRelationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Documents.DocumentRelation
        {
            Id = Guid.NewGuid(),
            DocumentId = request.DocumentId,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DocumentRelations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDocumentRelationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DocumentRelations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DocumentId = request.DocumentId;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.RelatedEntityId = request.RelatedEntityId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DocumentRelations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
