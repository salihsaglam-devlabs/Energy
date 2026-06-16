using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Reporting.ReportDefinition.Services;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;

namespace Energy.Infrastructure.Modules.Reporting.ReportDefinition.Services;

/// <summary>ReportDefinition CRUD servisi (projection, pagination, soft-delete).</summary>
public class ReportDefinitionService : IReportDefinitionService
{
    private readonly AppDbContext _db;

    public ReportDefinitionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>> GetListAsync(GetReportDefinitionListRequest request, CancellationToken ct = default)
    {
        var query = _db.ReportDefinitions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ReportDefinitionListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Module = e.Module,
                QueryKey = e.QueryKey,
                RequiredPermissionCode = e.RequiredPermissionCode,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ReportDefinitionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ReportDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ReportDefinitions.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ReportDefinitionDetailResponse
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
                Module = e.Module,
                QueryKey = e.QueryKey,
                RequiredPermissionCode = e.RequiredPermissionCode,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ReportDefinitionDetailResponse>.Failure("NotFound")
            : BaseResponse<ReportDefinitionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateReportDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Reporting.ReportDefinition
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Module = request.Module,
            QueryKey = request.QueryKey,
            RequiredPermissionCode = request.RequiredPermissionCode,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ReportDefinitions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReportDefinitionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ReportDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Module = request.Module;
            entity.QueryKey = request.QueryKey;
            entity.RequiredPermissionCode = request.RequiredPermissionCode;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ReportDefinitions.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
