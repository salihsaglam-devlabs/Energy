using Energy.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalStepApprover.Services;

/// <summary>ApprovalStepApprover CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalStepApproverService : IApprovalStepApproverService
{
    private readonly AppDbContext _db;

    public ApprovalStepApproverService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>> GetListAsync(GetApprovalStepApproverListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalStepApprovers.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalStepApproverListResponse
            {
                Id = e.Id,
                ApprovalStepDefinitionId = e.ApprovalStepDefinitionId,
                ApproverType = e.ApproverType,
                ApproverUserId = e.ApproverUserId,
                ApproverRoleId = e.ApproverRoleId,
                ApproverDepartmentId = e.ApproverDepartmentId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalStepApproverListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalStepApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalStepApprovers.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalStepApproverDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ApprovalStepDefinitionId = e.ApprovalStepDefinitionId,
                ApproverType = e.ApproverType,
                ApproverUserId = e.ApproverUserId,
                ApproverRoleId = e.ApproverRoleId,
                ApproverDepartmentId = e.ApproverDepartmentId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalStepApproverDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalStepApproverDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepApproverRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalStepApprover
        {
            Id = Guid.NewGuid(),
            ApprovalStepDefinitionId = request.ApprovalStepDefinitionId,
            ApproverType = request.ApproverType,
            ApproverUserId = request.ApproverUserId,
            ApproverRoleId = request.ApproverRoleId,
            ApproverDepartmentId = request.ApproverDepartmentId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalStepApprovers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepApproverRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalStepApprovers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalStepDefinitionId = request.ApprovalStepDefinitionId;
            entity.ApproverType = request.ApproverType;
            entity.ApproverUserId = request.ApproverUserId;
            entity.ApproverRoleId = request.ApproverRoleId;
            entity.ApproverDepartmentId = request.ApproverDepartmentId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalStepApprovers.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
