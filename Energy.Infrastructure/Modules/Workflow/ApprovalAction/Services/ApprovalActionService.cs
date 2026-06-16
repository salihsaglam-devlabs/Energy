using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalAction.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalAction.Services;

/// <summary>ApprovalAction CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalActionService : IApprovalActionService
{
    private readonly EnergyDbContext _db;

    public ApprovalActionService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalActionListResponse>>> GetListAsync(GetApprovalActionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalActions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalActionListResponse
            {
                Id = e.Id,
                ApprovalRequestId = e.ApprovalRequestId,
                ApprovalRequestStepId = e.ApprovalRequestStepId,
                UserId = e.UserId,
                ActionType = e.ActionType,
                ActionAt = e.ActionAt,
                Note = e.Note,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalActionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalActionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalActionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalActions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalActionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ApprovalRequestId = e.ApprovalRequestId,
                ApprovalRequestStepId = e.ApprovalRequestStepId,
                UserId = e.UserId,
                ActionType = e.ActionType,
                ActionAt = e.ActionAt,
                Note = e.Note
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalActionDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalActionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalActionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalAction
        {
            Id = Guid.NewGuid(),
            ApprovalRequestId = request.ApprovalRequestId,
            ApprovalRequestStepId = request.ApprovalRequestStepId,
            UserId = request.UserId,
            ActionType = request.ActionType,
            ActionAt = request.ActionAt,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalActions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalActionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalActions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalRequestId = request.ApprovalRequestId;
            entity.ApprovalRequestStepId = request.ApprovalRequestStepId;
            entity.UserId = request.UserId;
            entity.ActionType = request.ActionType;
            entity.ActionAt = request.ActionAt;
            entity.Note = request.Note;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalActions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
