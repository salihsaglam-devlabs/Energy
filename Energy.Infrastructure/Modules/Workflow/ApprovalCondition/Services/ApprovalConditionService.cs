using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalCondition.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalCondition.Services;

/// <summary>ApprovalCondition CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalConditionService : IApprovalConditionService
{
    private readonly AppDbContext _db;

    public ApprovalConditionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>> GetListAsync(GetApprovalConditionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalConditions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalConditionListResponse
            {
                Id = e.Id,
                ApprovalDefinitionVersionId = e.ApprovalDefinitionVersionId,
                FieldName = e.FieldName,
                Operator = e.Operator,
                ValueText = e.ValueText,
                ValueNumber = e.ValueNumber,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalConditionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalConditionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalConditions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalConditionDetailResponse
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
                FieldName = e.FieldName,
                Operator = e.Operator,
                ValueText = e.ValueText,
                ValueNumber = e.ValueNumber
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalConditionDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalConditionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalConditionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalCondition
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionVersionId = request.ApprovalDefinitionVersionId,
            FieldName = request.FieldName,
            Operator = request.Operator,
            ValueText = request.ValueText,
            ValueNumber = request.ValueNumber,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalConditions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalConditionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalConditions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalDefinitionVersionId = request.ApprovalDefinitionVersionId;
            entity.FieldName = request.FieldName;
            entity.Operator = request.Operator;
            entity.ValueText = request.ValueText;
            entity.ValueNumber = request.ValueNumber;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalConditions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
