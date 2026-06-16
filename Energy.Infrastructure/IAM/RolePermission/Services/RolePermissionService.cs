using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.RolePermission.Services;
using Energy.Shared.Models.V1.IAM.RolePermission.Requests;
using Energy.Shared.Models.V1.IAM.RolePermission.Responses;

namespace Energy.Infrastructure.IAM.RolePermission.Services;

/// <summary>
/// RolePermission: doğal/bileşik anahtarlı IAM kaydı. Liste/oluşturma desteklenir;
/// surrogate Guid ile yönetim parent/self-service ekranlarından yapılır.
/// </summary>
public class RolePermissionService : IRolePermissionService
{
    private readonly AppDbContext _db;

    public RolePermissionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<RolePermissionListResponse>>> GetListAsync(GetRolePermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.RolePermissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.RoleId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new RolePermissionListResponse
            {
                Id = Guid.Empty,
                RoleId = e.RoleId,
                PermissionCode = e.PermissionCode
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<RolePermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<RolePermissionListResponse>>.Success(page);
    }

    public Task<BaseResponse<RolePermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<RolePermissionDetailResponse>.Failure("NotSupported"));

    public async Task<BaseResponse<Guid>> CreateAsync(CreateRolePermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.IAM.RolePermission
        {
            RoleId = request.RoleId,
            PermissionCode = request.PermissionCode
        };
        _db.RolePermissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(Guid.Empty, "Created");
    }

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRolePermissionRequest request, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));
}
