using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.IAM.Permission.Services;
using Energy.Shared.Models.V1.IAM.Permission.Requests;
using Energy.Shared.Models.V1.IAM.Permission.Responses;

namespace Energy.Infrastructure.IAM.Permission.Services;

/// <summary>
/// Permission: doğal/bileşik anahtarlı IAM kaydı. Liste/oluşturma desteklenir;
/// surrogate Guid ile yönetim parent/self-service ekranlarından yapılır.
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly AppDbContext _db;

    public PermissionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PermissionListResponse>>> GetListAsync(GetPermissionListRequest request, CancellationToken ct = default)
    {
        var query = _db.Permissions.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PermissionListResponse
            {
                Id = Guid.Empty,
                Code = e.Code,
                Module = e.Module,
                Action = e.Action,
                DisplayNameKey = e.DisplayNameKey
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PermissionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PermissionListResponse>>.Success(page);
    }

    public Task<BaseResponse<PermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<PermissionDetailResponse>.Failure("NotSupported"));

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.IAM.Permission
        {
            Code = request.Code,
            Module = request.Module,
            Action = request.Action,
            DisplayNameKey = request.DisplayNameKey
        };
        _db.Permissions.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(Guid.Empty, "Created");
    }

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(BaseResponse<bool>.Failure("NotSupported"));
}
