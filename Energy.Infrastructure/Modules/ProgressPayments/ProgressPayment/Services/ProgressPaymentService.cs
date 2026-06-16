using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.ProgressPayments.ProgressPayment.Services;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;

namespace Energy.Infrastructure.Modules.ProgressPayments.ProgressPayment.Services;

/// <summary>ProgressPayment CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProgressPaymentService : IProgressPaymentService
{
    private readonly AppDbContext _db;

    public ProgressPaymentService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>> GetListAsync(GetProgressPaymentListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProgressPayments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProgressPaymentListResponse
            {
                Id = e.Id,
                ContractId = e.ContractId,
                PartnerId = e.PartnerId,
                ProgressPaymentNo = e.ProgressPaymentNo,
                PaymentPeriodStart = e.PaymentPeriodStart,
                PaymentPeriodEnd = e.PaymentPeriodEnd,
                GrossAmount = e.GrossAmount,
                DeductionTotal = e.DeductionTotal,
                NetAmount = e.NetAmount,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProgressPaymentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProgressPaymentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProgressPaymentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProgressPayments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProgressPaymentDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ContractId = e.ContractId,
                PartnerId = e.PartnerId,
                ProgressPaymentNo = e.ProgressPaymentNo,
                PaymentPeriodStart = e.PaymentPeriodStart,
                PaymentPeriodEnd = e.PaymentPeriodEnd,
                GrossAmount = e.GrossAmount,
                DeductionTotal = e.DeductionTotal,
                NetAmount = e.NetAmount,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProgressPaymentDetailResponse>.Failure("NotFound")
            : BaseResponse<ProgressPaymentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.ProgressPayments.ProgressPayment
        {
            Id = Guid.NewGuid(),
            ContractId = request.ContractId,
            PartnerId = request.PartnerId,
            ProgressPaymentNo = request.ProgressPaymentNo,
            PaymentPeriodStart = request.PaymentPeriodStart,
            PaymentPeriodEnd = request.PaymentPeriodEnd,
            GrossAmount = request.GrossAmount,
            DeductionTotal = request.DeductionTotal,
            NetAmount = request.NetAmount,
            Status = request.Status,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProgressPayments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProgressPayments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ContractId = request.ContractId;
            entity.PartnerId = request.PartnerId;
            entity.ProgressPaymentNo = request.ProgressPaymentNo;
            entity.PaymentPeriodStart = request.PaymentPeriodStart;
            entity.PaymentPeriodEnd = request.PaymentPeriodEnd;
            entity.GrossAmount = request.GrossAmount;
            entity.DeductionTotal = request.DeductionTotal;
            entity.NetAmount = request.NetAmount;
            entity.Status = request.Status;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProgressPayments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
