using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalDefinitionVersion.Services;

/// <summary>ApprovalDefinitionVersion CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalDefinitionVersionService : IApprovalDefinitionVersionService
{
    private readonly AppDbContext _db;

    public ApprovalDefinitionVersionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>> GetListAsync(GetApprovalDefinitionVersionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalDefinitionVersions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalDefinitionVersionListResponse
            {
                Id = e.Id,
                ApprovalDefinitionId = e.ApprovalDefinitionId,
                VersionNo = e.VersionNo,
                EffectiveFrom = e.EffectiveFrom,
                EffectiveTo = e.EffectiveTo,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalDefinitionVersionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalDefinitionVersionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalDefinitionVersions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalDefinitionVersionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                ApprovalDefinitionId = e.ApprovalDefinitionId,
                VersionNo = e.VersionNo,
                EffectiveFrom = e.EffectiveFrom,
                EffectiveTo = e.EffectiveTo,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalDefinitionVersionDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalDefinitionVersionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDefinitionVersionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion
        {
            Id = Guid.NewGuid(),
            ApprovalDefinitionId = request.ApprovalDefinitionId,
            VersionNo = request.VersionNo,
            EffectiveFrom = request.EffectiveFrom,
            EffectiveTo = request.EffectiveTo,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalDefinitionVersions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDefinitionVersionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalDefinitionVersions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.ApprovalDefinitionId = request.ApprovalDefinitionId;
            entity.VersionNo = request.VersionNo;
            entity.EffectiveFrom = request.EffectiveFrom;
            entity.EffectiveTo = request.EffectiveTo;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalDefinitionVersions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
