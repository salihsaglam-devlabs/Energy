using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Collection.Services;
using Energy.Shared.Models.V1.Finance.Collection.Requests;
using Energy.Shared.Models.V1.Finance.Collection.Responses;

namespace Energy.Infrastructure.Modules.Finance.Collection.Services;

/// <summary>Collection CRUD servisi (projection, pagination, soft-delete).</summary>
public class CollectionService : ICollectionService
{
    private readonly AppDbContext _db;

    public CollectionService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<CollectionListResponse>>> GetListAsync(GetCollectionListRequest request, CancellationToken ct = default)
    {
        var query = _db.Collections.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new CollectionListResponse
            {
                Id = e.Id,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                FinancialAccountId = e.FinancialAccountId,
                Amount = e.Amount,
                CollectionDate = e.CollectionDate,
                CollectionNo = e.CollectionNo,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<CollectionListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<CollectionListResponse>>.Success(page);
    }

    public async Task<BaseResponse<CollectionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Collections.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new CollectionDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                FinancialAccountId = e.FinancialAccountId,
                Amount = e.Amount,
                CollectionDate = e.CollectionDate,
                CollectionNo = e.CollectionNo,
                Status = e.Status,
                ApprovalRequestId = e.ApprovalRequestId
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<CollectionDetailResponse>.Failure("NotFound")
            : BaseResponse<CollectionDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateCollectionRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.Collection
        {
            Id = Guid.NewGuid(),
            PartnerId = request.PartnerId,
            CurrencyId = request.CurrencyId,
            FinancialAccountId = request.FinancialAccountId,
            Amount = request.Amount,
            CollectionDate = request.CollectionDate,
            CollectionNo = request.CollectionNo,
            Status = request.Status,
            ApprovalRequestId = request.ApprovalRequestId,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Collections.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCollectionRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Collections.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PartnerId = request.PartnerId;
            entity.CurrencyId = request.CurrencyId;
            entity.FinancialAccountId = request.FinancialAccountId;
            entity.Amount = request.Amount;
            entity.CollectionDate = request.CollectionDate;
            entity.CollectionNo = request.CollectionNo;
            entity.Status = request.Status;
            entity.ApprovalRequestId = request.ApprovalRequestId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Collections.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
