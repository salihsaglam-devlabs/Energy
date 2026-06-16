using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.UserRole.Services;
using Energy.Shared.Models.V1.IAM.UserRole.Requests;
using Energy.Shared.Models.V1.IAM.UserRole.Responses;

namespace Energy.Infrastructure.IAM.UserRole.Services;

/// <summary>
/// UserRole: doğal/bileşik anahtarlı IAM kaydı. Liste/oluşturma desteklenir;
/// surrogate Guid ile yönetim parent/self-service ekranlarından yapılır.
/// </summary>
public class UserRoleService : IUserRoleService
{
    private readonly AppDbContext _db;

    public UserRoleService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<UserRoleListResponse>>> GetListAsync(GetUserRoleListRequest request, CancellationToken ct = default)
    {
        var query = _db.UserRoles.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.UserId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new UserRoleListResponse
            {
                Id = Guid.Empty,
                UserId = e.UserId,
                RoleId = e.RoleId
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<UserRoleListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<UserRoleListResponse>>.Success(page);
    }

    public Task<BaseResponse<UserRoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<UserRoleDetailResponse>.Failure("NotSupported"));

    public async Task<BaseResponse<Guid>> CreateAsync(CreateUserRoleRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.IAM.UserRole
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };
        _db.UserRoles.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(Guid.Empty, "Created");
    }

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserRoleRequest request, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));
}
