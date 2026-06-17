using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.ProgressPayments.ProgressPaymentDeduction.Services;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;

namespace Energy.Infrastructure.ProgressPayments.ProgressPaymentDeduction.Services;

/// <summary>ProgressPaymentDeduction CRUD servisi (projection, pagination, soft-delete).</summary>
public class ProgressPaymentDeductionService : IProgressPaymentDeductionService
{
    private readonly AppDbContext _db;

    public ProgressPaymentDeductionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>> GetListAsync(GetProgressPaymentDeductionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ProgressPaymentDeductions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ProgressPaymentDeductionListResponse
            {
                Id = e.Id,
                ProgressPaymentId = e.ProgressPaymentId,
                DeductionType = e.DeductionType,
                Amount = e.Amount,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ProgressPaymentDeductionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ProgressPaymentDeductionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ProgressPaymentDeductions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ProgressPaymentDeductionDetailResponse
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
                DeductionType = e.DeductionType,
                Amount = e.Amount,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ProgressPaymentDeductionDetailResponse>.Failure("NotFound")
            : BaseResponse<ProgressPaymentDeductionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentDeductionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.ProgressPayments.ProgressPaymentDeduction
        {
            Id = Guid.NewGuid(),
            ProgressPaymentId = request.ProgressPaymentId,
            DeductionType = request.DeductionType,
            Amount = request.Amount,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ProgressPaymentDeductions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentDeductionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ProgressPaymentDeductions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ProgressPaymentId = request.ProgressPaymentId;
            entity.DeductionType = request.DeductionType;
            entity.Amount = request.Amount;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ProgressPaymentDeductions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
