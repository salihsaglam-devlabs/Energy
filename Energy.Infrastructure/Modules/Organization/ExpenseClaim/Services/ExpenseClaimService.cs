using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Organization.ExpenseClaim.Services;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

namespace Energy.Infrastructure.Modules.Organization.ExpenseClaim.Services;

/// <summary>ExpenseClaim CRUD servisi (projection, pagination, soft-delete).</summary>
public class ExpenseClaimService : IExpenseClaimService
{
    private readonly EnergyDbContext _db;

    public ExpenseClaimService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>> GetListAsync(GetExpenseClaimListRequest request, CancellationToken ct = default)
    {
        var query = _db.ExpenseClaims.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ExpenseClaimListResponse
            {
                Id = e.Id,
                EmployeeId = e.EmployeeId,
                ProjectId = e.ProjectId,
                CurrencyId = e.CurrencyId,
                ClaimNo = e.ClaimNo,
                ClaimDate = e.ClaimDate,
                TotalAmount = e.TotalAmount,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ExpenseClaimListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ExpenseClaimDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ExpenseClaims.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ExpenseClaimDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                EmployeeId = e.EmployeeId,
                ProjectId = e.ProjectId,
                CurrencyId = e.CurrencyId,
                ClaimNo = e.ClaimNo,
                ClaimDate = e.ClaimDate,
                TotalAmount = e.TotalAmount,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ExpenseClaimDetailResponse>.Failure("NotFound")
            : BaseResponse<ExpenseClaimDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateExpenseClaimRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Organization.ExpenseClaim
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            ProjectId = request.ProjectId,
            CurrencyId = request.CurrencyId,
            ClaimNo = request.ClaimNo,
            ClaimDate = request.ClaimDate,
            TotalAmount = request.TotalAmount,
            Status = request.Status,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ExpenseClaims.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExpenseClaimRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ExpenseClaims.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.EmployeeId = request.EmployeeId;
            entity.ProjectId = request.ProjectId;
            entity.CurrencyId = request.CurrencyId;
            entity.ClaimNo = request.ClaimNo;
            entity.ClaimDate = request.ClaimDate;
            entity.TotalAmount = request.TotalAmount;
            entity.Status = request.Status;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ExpenseClaims.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
