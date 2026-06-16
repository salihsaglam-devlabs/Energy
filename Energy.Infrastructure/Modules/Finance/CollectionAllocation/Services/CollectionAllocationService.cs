using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.CollectionAllocation.Services;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;

namespace Energy.Infrastructure.Modules.Finance.CollectionAllocation.Services;

/// <summary>CollectionAllocation CRUD servisi (projection, pagination, soft-delete).</summary>
public class CollectionAllocationService : ICollectionAllocationService
{
    private readonly AppDbContext _db;

    public CollectionAllocationService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>> GetListAsync(GetCollectionAllocationListRequest request, CancellationToken ct = default)
    {
        var query = _db.CollectionAllocations.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new CollectionAllocationListResponse
            {
                Id = e.Id,
                CollectionId = e.CollectionId,
                ReceivableId = e.ReceivableId,
                Amount = e.Amount,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<CollectionAllocationListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>.Success(page);
    }

    public async Task<BaseResponse<CollectionAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.CollectionAllocations.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new CollectionAllocationDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CollectionId = e.CollectionId,
                ReceivableId = e.ReceivableId,
                Amount = e.Amount
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<CollectionAllocationDetailResponse>.Failure("NotFound")
            : BaseResponse<CollectionAllocationDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateCollectionAllocationRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.CollectionAllocation
        {
            Id = Guid.NewGuid(),
            CollectionId = request.CollectionId,
            ReceivableId = request.ReceivableId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow,
        };
        _db.CollectionAllocations.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCollectionAllocationRequest request, CancellationToken ct = default)
    {
        var entity = await _db.CollectionAllocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CollectionId = request.CollectionId;
            entity.ReceivableId = request.ReceivableId;
            entity.Amount = request.Amount;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.CollectionAllocations.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
