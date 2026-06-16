using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Payment.Services;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using Energy.Shared.Models.V1.Finance.Payment.Responses;

namespace Energy.Infrastructure.Modules.Finance.Payment.Services;

/// <summary>Payment CRUD servisi (projection, pagination, soft-delete).</summary>
public class PaymentService : IPaymentService
{
    private readonly EnergyDbContext _db;

    public PaymentService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PaymentListResponse>>> GetListAsync(GetPaymentListRequest request, CancellationToken ct = default)
    {
        var query = _db.Payments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PaymentListResponse
            {
                Id = e.Id,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                FinancialAccountId = e.FinancialAccountId,
                Amount = e.Amount,
                PaymentDate = e.PaymentDate,
                PaymentNo = e.PaymentNo,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PaymentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PaymentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PaymentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Payments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PaymentDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                FinancialAccountId = e.FinancialAccountId,
                Amount = e.Amount,
                PaymentDate = e.PaymentDate,
                PaymentNo = e.PaymentNo,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PaymentDetailResponse>.Failure("NotFound")
            : BaseResponse<PaymentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.Payment
        {
            Id = Guid.NewGuid(),
            PartnerId = request.PartnerId,
            CurrencyId = request.CurrencyId,
            FinancialAccountId = request.FinancialAccountId,
            Amount = request.Amount,
            PaymentDate = request.PaymentDate,
            PaymentNo = request.PaymentNo,
            Status = request.Status,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Payments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Payments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PartnerId = request.PartnerId;
            entity.CurrencyId = request.CurrencyId;
            entity.FinancialAccountId = request.FinancialAccountId;
            entity.Amount = request.Amount;
            entity.PaymentDate = request.PaymentDate;
            entity.PaymentNo = request.PaymentNo;
            entity.Status = request.Status;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Payments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
