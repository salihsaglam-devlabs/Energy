using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Services;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;

namespace Energy.Infrastructure.Modules.Procurement.SupplierInvoiceLine.Services;

/// <summary>SupplierInvoiceLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class SupplierInvoiceLineService : ISupplierInvoiceLineService
{
    private readonly AppDbContext _db;

    public SupplierInvoiceLineService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>> GetListAsync(GetSupplierInvoiceLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.SupplierInvoiceLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new SupplierInvoiceLineListResponse
            {
                Id = e.Id,
                SupplierInvoiceId = e.SupplierInvoiceId,
                MaterialId = e.MaterialId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                TaxRate = e.TaxRate,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<SupplierInvoiceLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<SupplierInvoiceLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.SupplierInvoiceLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new SupplierInvoiceLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                SupplierInvoiceId = e.SupplierInvoiceId,
                MaterialId = e.MaterialId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                TaxRate = e.TaxRate
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<SupplierInvoiceLineDetailResponse>.Failure("NotFound")
            : BaseResponse<SupplierInvoiceLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Procurement.SupplierInvoiceLine
        {
            Id = Guid.NewGuid(),
            SupplierInvoiceId = request.SupplierInvoiceId,
            MaterialId = request.MaterialId,
            Description = request.Description,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            TaxRate = request.TaxRate,
            CreatedAt = DateTime.UtcNow,
        };
        _db.SupplierInvoiceLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.SupplierInvoiceLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.SupplierInvoiceId = request.SupplierInvoiceId;
            entity.MaterialId = request.MaterialId;
            entity.Description = request.Description;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
            entity.TaxRate = request.TaxRate;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SupplierInvoiceLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
