using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalDelegation.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalDelegation.Services;

/// <summary>ApprovalDelegation CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalDelegationService : IApprovalDelegationService
{
    private readonly AppDbContext _db;

    public ApprovalDelegationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>> GetListAsync(GetApprovalDelegationListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalDelegations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalDelegationListResponse
            {
                Id = e.Id,
                DelegatorUserId = e.DelegatorUserId,
                DelegateUserId = e.DelegateUserId,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalDelegationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalDelegationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalDelegations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalDelegationDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                DelegatorUserId = e.DelegatorUserId,
                DelegateUserId = e.DelegateUserId,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalDelegationDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalDelegationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDelegationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Workflow.ApprovalDelegation
        {
            Id = Guid.NewGuid(),
            DelegatorUserId = request.DelegatorUserId,
            DelegateUserId = request.DelegateUserId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalDelegations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDelegationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalDelegations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.DelegatorUserId = request.DelegatorUserId;
            entity.DelegateUserId = request.DelegateUserId;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalDelegations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
