using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Payable.Services;
using Energy.Shared.Models.V1.Finance.Payable.Requests;
using Energy.Shared.Models.V1.Finance.Payable.Responses;

namespace Energy.Infrastructure.Modules.Finance.Payable.Services;

/// <summary>Payable CRUD servisi (projection, pagination, soft-delete).</summary>
public class PayableService : IPayableService
{
    private readonly EnergyDbContext _db;

    public PayableService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<PayableListResponse>>> GetListAsync(GetPayableListRequest request, CancellationToken ct = default)
    {
        var query = _db.Payables.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new PayableListResponse
            {
                Id = e.Id,
                PartnerId = e.PartnerId,
                CurrencyId = e.CurrencyId,
                Amount = e.Amount,
                RemainingAmount = e.RemainingAmount,
                DueDate = e.DueDate,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                IsClosed = e.IsClosed,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<PayableListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<PayableListResponse>>.Success(page);
    }

    public async Task<BaseResponse<PayableDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Payables.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new PayableDetailResponse
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
                Amount = e.Amount,
                RemainingAmount = e.RemainingAmount,
                DueDate = e.DueDate,
                RelatedModule = e.RelatedModule,
                RelatedEntityType = e.RelatedEntityType,
                RelatedEntityId = e.RelatedEntityId,
                IsClosed = e.IsClosed
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<PayableDetailResponse>.Failure("NotFound")
            : BaseResponse<PayableDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreatePayableRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.Payable
        {
            Id = Guid.NewGuid(),
            PartnerId = request.PartnerId,
            CurrencyId = request.CurrencyId,
            Amount = request.Amount,
            RemainingAmount = request.RemainingAmount,
            DueDate = request.DueDate,
            RelatedModule = request.RelatedModule,
            RelatedEntityType = request.RelatedEntityType,
            RelatedEntityId = request.RelatedEntityId,
            IsClosed = request.IsClosed,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Payables.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePayableRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Payables.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.PartnerId = request.PartnerId;
            entity.CurrencyId = request.CurrencyId;
            entity.Amount = request.Amount;
            entity.RemainingAmount = request.RemainingAmount;
            entity.DueDate = request.DueDate;
            entity.RelatedModule = request.RelatedModule;
            entity.RelatedEntityType = request.RelatedEntityType;
            entity.RelatedEntityId = request.RelatedEntityId;
            entity.IsClosed = request.IsClosed;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.Payables.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
