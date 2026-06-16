using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockDocument.Services;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockDocument.Services;

/// <summary>StockDocument CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockDocumentService : IStockDocumentService
{
    private readonly AppDbContext _db;

    public StockDocumentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockDocumentListResponse>>> GetListAsync(GetStockDocumentListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockDocuments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockDocumentListResponse
            {
                Id = e.Id,
                DocumentTypeId = e.DocumentTypeId,
                SourceWarehouseId = e.SourceWarehouseId,
                TargetWarehouseId = e.TargetWarehouseId,
                ProjectId = e.ProjectId,
                Status = e.Status,
                DocumentNo = e.DocumentNo,
                DocumentDate = e.DocumentDate,
                Note = e.Note,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockDocumentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockDocumentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockDocumentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockDocuments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockDocumentDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                DocumentTypeId = e.DocumentTypeId,
                SourceWarehouseId = e.SourceWarehouseId,
                TargetWarehouseId = e.TargetWarehouseId,
                ProjectId = e.ProjectId,
                Status = e.Status,
                DocumentNo = e.DocumentNo,
                DocumentDate = e.DocumentDate,
                Note = e.Note,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockDocumentDetailResponse>.Failure("NotFound")
            : BaseResponse<StockDocumentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockDocument
        {
            Id = Guid.NewGuid(),
            DocumentTypeId = request.DocumentTypeId,
            SourceWarehouseId = request.SourceWarehouseId,
            TargetWarehouseId = request.TargetWarehouseId,
            ProjectId = request.ProjectId,
            Status = request.Status,
            DocumentNo = request.DocumentNo,
            DocumentDate = request.DocumentDate,
            Note = request.Note,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockDocuments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockDocuments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DocumentTypeId = request.DocumentTypeId;
            entity.SourceWarehouseId = request.SourceWarehouseId;
            entity.TargetWarehouseId = request.TargetWarehouseId;
            entity.ProjectId = request.ProjectId;
            entity.Status = request.Status;
            entity.DocumentNo = request.DocumentNo;
            entity.DocumentDate = request.DocumentDate;
            entity.Note = request.Note;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockDocuments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
