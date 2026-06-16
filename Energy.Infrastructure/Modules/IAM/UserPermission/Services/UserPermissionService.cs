using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.UserPermission.Services;
using Energy.Shared.Models.V1.IAM.UserPermission.Requests;
using Energy.Shared.Models.V1.IAM.UserPermission.Responses;

namespace Energy.Infrastructure.Modules.IAM.UserPermission.Services;

/// <summary>
/// UserPermission: doğal/bileşik anahtarlı IAM kaydı. Liste/oluşturma desteklenir;
/// surrogate Guid ile yönetim parent/self-service ekranlarından yapılır.
/// </summary>
public class UserPermissionService : IUserPermissionService
{
    private readonly AppDbContext _db;

    public UserPermissionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UserPermissionListResponse>>> GetListAsync(GetUserPermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.UserPermissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.UserId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UserPermissionListResponse
            {
                Id = Guid.Empty,
                UserId = e.UserId,
                PermissionCode = e.PermissionCode
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UserPermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UserPermissionListResponse>>.Success(page);
    }

    public Task<BaseResponse<UserPermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<UserPermissionDetailResponse>.Failure("NotSupported"));

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUserPermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.IAM.UserPermission
        {
            UserId = request.UserId,
            PermissionCode = request.PermissionCode
        };
        _db.UserPermissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(Guid.Empty, "Created");
    }

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserPermissionRequest request, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));
}
