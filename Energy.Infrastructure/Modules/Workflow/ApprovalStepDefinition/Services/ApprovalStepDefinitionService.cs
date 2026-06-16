using Energy.Shared.Common;
using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalStepDefinition.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalStepDefinition.Services;

/// <summary>ApprovalStepDefinition CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalStepDefinitionService : IApprovalStepDefinitionService
{
    private readonly AppDbContext _db;

    public ApprovalStepDefinitionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>> GetListAsync(GetApprovalStepDefinitionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalStepDefinitions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalStepDefinitionListResponse
            {
                Id = e.Id,
                ApprovalDefinitionVersionId = e.ApprovalDefinitionVersionId,
                StepNo = e.StepNo,
                ApprovalMode = e.ApprovalMode,
                RequiredApprovalCount = e.RequiredApprovalCount,
                IsRequired = e.IsRequired,
                Name = e.Name,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalStepDefinitionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalStepDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalStepDefinitions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalStepDefinitionDetailResponse
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
                StepNo = e.StepNo,
                ApprovalMode = e.ApprovalMode,
                RequiredApprovalCount = e.RequiredApprovalCount,
                IsRequired = e.IsRequired,
                Name = e.Name
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalStepDefinitionDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalStepDefinitionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionVersionId = request.ApprovalDefinitionVersionId,
            StepNo = request.StepNo,
            ApprovalMode = request.ApprovalMode,
            RequiredApprovalCount = request.RequiredApprovalCount,
            IsRequired = request.IsRequired,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalStepDefinitions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalStepDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalDefinitionVersionId = request.ApprovalDefinitionVersionId;
            entity.StepNo = request.StepNo;
            entity.ApprovalMode = request.ApprovalMode;
            entity.RequiredApprovalCount = request.RequiredApprovalCount;
            entity.IsRequired = request.IsRequired;
            entity.Name = request.Name;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalStepDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
