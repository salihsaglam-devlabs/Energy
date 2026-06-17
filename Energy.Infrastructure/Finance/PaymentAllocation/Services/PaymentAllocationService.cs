using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.PaymentAllocation.Services;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

namespace Energy.Infrastructure.Finance.PaymentAllocation.Services;

/// <summary>PaymentAllocation CRUD servisi (projection, pagination, soft-delete).</summary>
public class PaymentAllocationService : IPaymentAllocationService
{
    private readonly AppDbContext _db;

    public PaymentAllocationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>> GetListAsync(GetPaymentAllocationListRequest request, CancellationToken ct = default)
    {
        var query = _db.PaymentAllocations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PaymentAllocationListResponse
            {
                Id = e.Id,
                PaymentId = e.PaymentId,
                PayableId = e.PayableId,
                Amount = e.Amount,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PaymentAllocationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PaymentAllocationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PaymentAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.PaymentAllocations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PaymentAllocationDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                PaymentId = e.PaymentId,
                PayableId = e.PayableId,
                Amount = e.Amount
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PaymentAllocationDetailResponse>.Failure("NotFound")
            : BaseResponse<PaymentAllocationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePaymentAllocationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Finance.PaymentAllocation
        {
            Id = Guid.NewGuid(),
            PaymentId = request.PaymentId,
            PayableId = request.PayableId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow,
        };
        _db.PaymentAllocations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePaymentAllocationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.PaymentAllocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PaymentId = request.PaymentId;
            entity.PayableId = request.PayableId;
            entity.Amount = request.Amount;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.PaymentAllocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
