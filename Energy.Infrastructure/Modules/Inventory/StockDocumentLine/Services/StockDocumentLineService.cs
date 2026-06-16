using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockDocumentLine.Services;

/// <summary>StockDocumentLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class StockDocumentLineService : IStockDocumentLineService
{
    private readonly AppDbContext _db;

    public StockDocumentLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>> GetListAsync(GetStockDocumentLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.StockDocumentLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new StockDocumentLineListResponse
            {
                Id = e.Id,
                StockDocumentId = e.StockDocumentId,
                MaterialId = e.MaterialId,
                UnitOfMeasureId = e.UnitOfMeasureId,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CurrencyId = e.CurrencyId,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<StockDocumentLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<StockDocumentLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.StockDocumentLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new StockDocumentLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                StockDocumentId = e.StockDocumentId,
                MaterialId = e.MaterialId,
                UnitOfMeasureId = e.UnitOfMeasureId,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                CurrencyId = e.CurrencyId,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<StockDocumentLineDetailResponse>.Failure("NotFound")
            : BaseResponse<StockDocumentLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Inventory.StockDocumentLine
        {
            Id = Guid.NewGuid(),
            StockDocumentId = request.StockDocumentId,
            MaterialId = request.MaterialId,
            UnitOfMeasureId = request.UnitOfMeasureId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            CurrencyId = request.CurrencyId,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.StockDocumentLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.StockDocumentLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.StockDocumentId = request.StockDocumentId;
            entity.MaterialId = request.MaterialId;
            entity.UnitOfMeasureId = request.UnitOfMeasureId;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
            entity.CurrencyId = request.CurrencyId;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.StockDocumentLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
