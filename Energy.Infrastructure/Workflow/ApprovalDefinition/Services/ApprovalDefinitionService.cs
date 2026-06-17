using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalDefinition.Services;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalDefinition.Services;

/// <summary>ApprovalDefinition CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApprovalDefinitionService : IApprovalDefinitionService
{
    private readonly AppDbContext _db;

    public ApprovalDefinitionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApprovalDefinitionListResponse>>> GetListAsync(GetApprovalDefinitionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApprovalDefinitions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApprovalDefinitionListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApprovalDefinitionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApprovalDefinitionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApprovalDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApprovalDefinitions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApprovalDefinitionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Code = e.Code,
                Name = e.Name,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApprovalDefinitionDetailResponse>.Failure("NotFound")
            : BaseResponse<ApprovalDefinitionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Workflow.ApprovalDefinition
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApprovalDefinitions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApprovalDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
