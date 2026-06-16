using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Requests.RequestType.Services;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;

namespace Energy.Infrastructure.Modules.Requests.RequestType.Services;

/// <summary>RequestType CRUD servisi (projection, pagination, soft-delete).</summary>
public class RequestTypeService : IRequestTypeService
{
    private readonly EnergyDbContext _db;

    public RequestTypeService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<RequestTypeListResponse>>> GetListAsync(GetRequestTypeListRequest request, CancellationToken ct = default)
    {
        var query = _db.RequestTypes.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new RequestTypeListResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Category = e.Category,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<RequestTypeListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<RequestTypeListResponse>>.Success(page);
    }

    public async Task<BaseResponse<RequestTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.RequestTypes.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new RequestTypeDetailResponse
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
                Category = e.Category,
                IsActive = e.IsActive
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<RequestTypeDetailResponse>.Failure("NotFound")
            : BaseResponse<RequestTypeDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateRequestTypeRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Requests.RequestType
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Category = request.Category,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };
        _db.RequestTypes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRequestTypeRequest request, CancellationToken ct = default)
    {
        var entity = await _db.RequestTypes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.Category = request.Category;
            entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.RequestTypes.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
