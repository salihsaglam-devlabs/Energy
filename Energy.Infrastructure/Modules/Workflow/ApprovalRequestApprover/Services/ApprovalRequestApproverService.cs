using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalRequestApprover.Services;

/// <summary>ApprovalRequestApprover CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalRequestApproverService : IApprovalRequestApproverService
{
    private readonly AppDbContext _db;

    public ApprovalRequestApproverService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>> GetListAsync(GetApprovalRequestApproverListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequestApprovers.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalRequestApproverListResponse
            {
                Id = e.Id,
                ApprovalRequestStepId = e.ApprovalRequestStepId,
                UserId = e.UserId,
                Status = e.Status,
                ActionAt = e.ActionAt,
                DelegatedFromUserId = e.DelegatedFromUserId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalRequestApproverListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalRequestApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalRequestApprovers.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalRequestApproverDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ApprovalRequestStepId = e.ApprovalRequestStepId,
                UserId = e.UserId,
                Status = e.Status,
                ActionAt = e.ActionAt,
                DelegatedFromUserId = e.DelegatedFromUserId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalRequestApproverDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalRequestApproverDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestApproverRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalRequestApprover
        {
            Id = Guid.NewGuid(),
            ApprovalRequestStepId = request.ApprovalRequestStepId,
            UserId = request.UserId,
            Status = request.Status,
            ActionAt = request.ActionAt,
            DelegatedFromUserId = request.DelegatedFromUserId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalRequestApprovers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestApproverRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalRequestApprovers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalRequestStepId = request.ApprovalRequestStepId;
            entity.UserId = request.UserId;
            entity.Status = request.Status;
            entity.ActionAt = request.ActionAt;
            entity.DelegatedFromUserId = request.DelegatedFromUserId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalRequestApprovers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
