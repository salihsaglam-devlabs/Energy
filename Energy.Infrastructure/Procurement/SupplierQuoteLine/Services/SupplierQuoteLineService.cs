using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.SupplierQuoteLine.Services;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Infrastructure.Procurement.SupplierQuoteLine.Services;

/// <summary>SupplierQuoteLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class SupplierQuoteLineService : ISupplierQuoteLineService
{
    private readonly AppDbContext _db;

    public SupplierQuoteLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>> GetListAsync(GetSupplierQuoteLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.SupplierQuoteLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new SupplierQuoteLineListResponse
            {
                Id = e.Id,
                SupplierQuoteId = e.SupplierQuoteId,
                RequestLineId = e.RequestLineId,
                MaterialId = e.MaterialId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                TaxRate = e.TaxRate,
                DiscountRate = e.DiscountRate,
                DeliveryDays = e.DeliveryDays,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<SupplierQuoteLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<SupplierQuoteLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.SupplierQuoteLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new SupplierQuoteLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                SupplierQuoteId = e.SupplierQuoteId,
                RequestLineId = e.RequestLineId,
                MaterialId = e.MaterialId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                TaxRate = e.TaxRate,
                DiscountRate = e.DiscountRate,
                DeliveryDays = e.DeliveryDays
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<SupplierQuoteLineDetailResponse>.Failure("NotFound")
            : BaseResponse<SupplierQuoteLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateSupplierQuoteLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Procurement.SupplierQuoteLine
        {
            Id = Guid.NewGuid(),
            SupplierQuoteId = request.SupplierQuoteId,
            RequestLineId = request.RequestLineId,
            MaterialId = request.MaterialId,
            Description = request.Description,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            TaxRate = request.TaxRate,
            DiscountRate = request.DiscountRate,
            DeliveryDays = request.DeliveryDays,
            CreatedAt = DateTime.UtcNow,
        };
        _db.SupplierQuoteLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierQuoteLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.SupplierQuoteLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SupplierQuoteId = request.SupplierQuoteId;
            entity.RequestLineId = request.RequestLineId;
            entity.MaterialId = request.MaterialId;
            entity.Description = request.Description;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
            entity.TaxRate = request.TaxRate;
            entity.DiscountRate = request.DiscountRate;
            entity.DeliveryDays = request.DeliveryDays;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SupplierQuoteLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
