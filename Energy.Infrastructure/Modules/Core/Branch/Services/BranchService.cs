using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Branch.Services;
using Energy.Shared.Models.V1.Core.Branch.Requests;
using Energy.Shared.Models.V1.Core.Branch.Responses;

namespace Energy.Infrastructure.Modules.Core.Branch.Services;

/// <summary>Branch CRUD servisi (projection, pagination, soft-delete).</summary>
public class BranchService : IBranchService
{
    private readonly EnergyDbContext _db;

    public BranchService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<BranchListResponse>>> GetListAsync(GetBranchListRequest request, CancellationToken ct = default)
    {
        var query = _db.Branches.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new BranchListResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                Code = e.Code,
                Name = e.Name,
                Address = e.Address,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<BranchListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<BranchListResponse>>.Success(page);
    }

    public async Task<BaseResponse<BranchDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Branches.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new BranchDetailResponse
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
                Code = e.Code,
                Name = e.Name,
                Address = e.Address,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<BranchDetailResponse>.Failure("NotFound")
            : BaseResponse<BranchDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateBranchRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.Branch
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            Code = request.Code,
            Name = request.Name,
            Address = request.Address,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Branches.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBranchRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Branches.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CompanyId = request.CompanyId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Address = request.Address;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Branches.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
