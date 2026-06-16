using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Department.Services;
using Energy.Shared.Models.V1.Core.Department.Requests;
using Energy.Shared.Models.V1.Core.Department.Responses;

namespace Energy.Infrastructure.Modules.Core.Department.Services;

/// <summary>Department CRUD servisi (projection, pagination, soft-delete).</summary>
public class DepartmentService : IDepartmentService
{
    private readonly EnergyDbContext _db;

    public DepartmentService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<DepartmentListResponse>>> GetListAsync(GetDepartmentListRequest request, CancellationToken ct = default)
    {
        var query = _db.Departments.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new DepartmentListResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                ParentDepartmentId = e.ParentDepartmentId,
                Code = e.Code,
                Name = e.Name,
                ManagerUserId = e.ManagerUserId,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<DepartmentListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<DepartmentListResponse>>.Success(page);
    }

    public async Task<BaseResponse<DepartmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Departments.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new DepartmentDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CompanyId = e.CompanyId,
                ParentDepartmentId = e.ParentDepartmentId,
                Code = e.Code,
                Name = e.Name,
                ManagerUserId = e.ManagerUserId,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<DepartmentDetailResponse>.Failure("NotFound")
            : BaseResponse<DepartmentDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.Department
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            ParentDepartmentId = request.ParentDepartmentId,
            Code = request.Code,
            Name = request.Name,
            ManagerUserId = request.ManagerUserId,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Departments.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Departments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CompanyId = request.CompanyId;
            entity.ParentDepartmentId = request.ParentDepartmentId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.ManagerUserId = request.ManagerUserId;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Departments.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
