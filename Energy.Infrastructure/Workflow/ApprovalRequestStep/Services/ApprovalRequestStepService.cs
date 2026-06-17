using Energy.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalRequestStep.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalRequestStep.Services;

/// <summary>ApprovalRequestStep CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalRequestStepService : IApprovalRequestStepService
{
    private readonly AppDbContext _db;

    public ApprovalRequestStepService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>> GetListAsync(GetApprovalRequestStepListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequestSteps.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalRequestStepListResponse
            {
                Id = e.Id,
                ApprovalRequestId = e.ApprovalRequestId,
                ApprovalStepDefinitionId = e.ApprovalStepDefinitionId,
                StepNo = e.StepNo,
                Status = e.Status,
                ApprovalMode = e.ApprovalMode,
                RequiredApprovalCount = e.RequiredApprovalCount,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalRequestStepListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalRequestStepDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalRequestSteps.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalRequestStepDetailResponse
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
                ApprovalStepDefinitionId = e.ApprovalStepDefinitionId,
                StepNo = e.StepNo,
                Status = e.Status,
                ApprovalMode = e.ApprovalMode,
                RequiredApprovalCount = e.RequiredApprovalCount
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalRequestStepDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalRequestStepDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestStepRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Workflow.ApprovalRequestStep
        {
            Id = Guid.NewGuid(),
            ApprovalRequestId = request.ApprovalRequestId,
            ApprovalStepDefinitionId = request.ApprovalStepDefinitionId,
            StepNo = request.StepNo,
            Status = request.Status,
            ApprovalMode = request.ApprovalMode,
            RequiredApprovalCount = request.RequiredApprovalCount,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalRequestSteps.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestStepRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalRequestSteps.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalRequestId = request.ApprovalRequestId;
            entity.ApprovalStepDefinitionId = request.ApprovalStepDefinitionId;
            entity.StepNo = request.StepNo;
            entity.Status = request.Status;
            entity.ApprovalMode = request.ApprovalMode;
            entity.RequiredApprovalCount = request.RequiredApprovalCount;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalRequestSteps.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
