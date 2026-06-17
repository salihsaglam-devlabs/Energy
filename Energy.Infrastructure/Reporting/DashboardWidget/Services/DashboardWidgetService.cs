using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Reporting.DashboardWidget.Services;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;

namespace Energy.Infrastructure.Reporting.DashboardWidget.Services;

/// <summary>DashboardWidget CRUD servisi (projection, pagination, soft-delete).</summary>
public class DashboardWidgetService : IDashboardWidgetService
{
    private readonly AppDbContext _db;

    public DashboardWidgetService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>> GetListAsync(GetDashboardWidgetListRequest request, CancellationToken ct = default)
    {
        var query = _db.DashboardWidgets.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DashboardWidgetListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Module = e.Module,
                WidgetType = e.WidgetType,
                RequiredPermissionCode = e.RequiredPermissionCode,
                DisplayOrder = e.DisplayOrder,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DashboardWidgetListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DashboardWidgetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.DashboardWidgets.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DashboardWidgetDetailResponse
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
                WidgetType = e.WidgetType,
                RequiredPermissionCode = e.RequiredPermissionCode,
                DisplayOrder = e.DisplayOrder,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DashboardWidgetDetailResponse>.Failure("NotFound")
            : BaseResponse<DashboardWidgetDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDashboardWidgetRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Reporting.DashboardWidget
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Module = request.Module,
            WidgetType = request.WidgetType,
            RequiredPermissionCode = request.RequiredPermissionCode,
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.DashboardWidgets.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDashboardWidgetRequest request, CancellationToken ct = default)
    {
        var entity = await _db.DashboardWidgets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Module = request.Module;
            entity.WidgetType = request.WidgetType;
            entity.RequiredPermissionCode = request.RequiredPermissionCode;
            entity.DisplayOrder = request.DisplayOrder;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.DashboardWidgets.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
