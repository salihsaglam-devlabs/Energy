using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.ApiEndpoint.Services;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Requests;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

namespace Energy.Infrastructure.Modules.IAM.ApiEndpoint.Services;

/// <summary>ApiEndpoint CRUD servisi (projection, pagination, soft-delete).</summary>
public class ApiEndpointService : IApiEndpointService
{
    private readonly AppDbContext _db;

    public ApiEndpointService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ApiEndpointListResponse>>> GetListAsync(GetApiEndpointListRequest request, CancellationToken ct = default)
    {
        var query = _db.ApiEndpoints.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ApiEndpointListResponse
            {
                Id = e.Id,
                Path = e.Path,
                HttpMethod = e.HttpMethod,
                RequiredPermissionCode = e.RequiredPermissionCode,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ApiEndpointListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ApiEndpointListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ApiEndpointDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ApiEndpoints.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ApiEndpointDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                Path = e.Path,
                HttpMethod = e.HttpMethod,
                RequiredPermissionCode = e.RequiredPermissionCode
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ApiEndpointDetailResponse>.Failure("NotFound")
            : BaseResponse<ApiEndpointDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateApiEndpointRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.ApiEndpoint
        {
            Id = Guid.NewGuid(),
            Path = request.Path,
            HttpMethod = request.HttpMethod,
            RequiredPermissionCode = request.RequiredPermissionCode,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ApiEndpoints.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ApiEndpoints.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Path = request.Path;
            entity.HttpMethod = request.HttpMethod;
            entity.RequiredPermissionCode = request.RequiredPermissionCode;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ApiEndpoints.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
