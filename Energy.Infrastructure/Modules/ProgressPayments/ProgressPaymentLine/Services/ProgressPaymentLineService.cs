using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Services;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;

namespace Energy.Infrastructure.Modules.ProgressPayments.ProgressPaymentLine.Services;

/// <summary>ProgressPaymentLine CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProgressPaymentLineService : IProgressPaymentLineService
{
    private readonly EnergyDbContext _db;

    public ProgressPaymentLineService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>> GetListAsync(GetProgressPaymentLineListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProgressPaymentLines.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProgressPaymentLineListResponse
            {
                Id = e.Id,
                ProgressPaymentId = e.ProgressPaymentId,
                ContractLineId = e.ContractLineId,
                MeasurementSheetLineId = e.MeasurementSheetLineId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                Amount = e.Amount,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProgressPaymentLineListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProgressPaymentLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProgressPaymentLines.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProgressPaymentLineDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ProgressPaymentId = e.ProgressPaymentId,
                ContractLineId = e.ContractLineId,
                MeasurementSheetLineId = e.MeasurementSheetLineId,
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice,
                Amount = e.Amount
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProgressPaymentLineDetailResponse>.Failure("NotFound")
            : BaseResponse<ProgressPaymentLineDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentLineRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentLine
        {
            Id = Guid.NewGuid(),
            ProgressPaymentId = request.ProgressPaymentId,
            ContractLineId = request.ContractLineId,
            MeasurementSheetLineId = request.MeasurementSheetLineId,
            Description = request.Description,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProgressPaymentLines.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentLineRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProgressPaymentLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProgressPaymentId = request.ProgressPaymentId;
            entity.ContractLineId = request.ContractLineId;
            entity.MeasurementSheetLineId = request.MeasurementSheetLineId;
            entity.Description = request.Description;
            entity.Quantity = request.Quantity;
            entity.UnitPrice = request.UnitPrice;
            entity.Amount = request.Amount;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProgressPaymentLines.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
