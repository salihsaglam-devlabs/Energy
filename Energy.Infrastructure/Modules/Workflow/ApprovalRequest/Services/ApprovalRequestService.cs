using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalRequest.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalRequest.Services;

/// <summary>ApprovalRequest CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalRequestService : IApprovalRequestService
{
    private readonly EnergyDbContext _db;

    public ApprovalRequestService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>> GetListAsync(GetApprovalRequestListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequests.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalRequestListResponse
            {
                Id = e.Id,
                ApprovalDefinitionVersionId = e.ApprovalDefinitionVersionId,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                RequestedByUserId = e.RequestedByUserId,
                Status = e.Status,
                CurrentStepNo = e.CurrentStepNo,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalRequestListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalRequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalRequests.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalRequestDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ApprovalDefinitionVersionId = e.ApprovalDefinitionVersionId,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                RequestedByUserId = e.RequestedByUserId,
                Status = e.Status,
                CurrentStepNo = e.CurrentStepNo
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalRequestDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalRequestDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalRequest
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionVersionId = request.ApprovalDefinitionVersionId,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            RequestedByUserId = request.RequestedByUserId,
            Status = request.Status,
            CurrentStepNo = request.CurrentStepNo,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalRequests.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalRequests.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalDefinitionVersionId = request.ApprovalDefinitionVersionId;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.RelatedEntityId = request.RelatedEntityId;
            entity.RequestedByUserId = request.RequestedByUserId;
            entity.Status = request.Status;
            entity.CurrentStepNo = request.CurrentStepNo;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalRequests.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
