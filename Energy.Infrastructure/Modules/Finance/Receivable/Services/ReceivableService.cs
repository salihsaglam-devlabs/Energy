using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Receivable.Services;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;

namespace Energy.Infrastructure.Modules.Finance.Receivable.Services;

/// <summary>Receivable CRUD servisi (projection, pagination, soft-delete).</summary>
public class ReceivableService : IReceivableService
{
    private readonly EnergyDbContext _db;

    public ReceivableService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ReceivableListResponse>>> GetListAsync(GetReceivableListRequest request, CancellationToken ct = default)
    {
        var query = _db.Receivables.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ReceivableListResponse
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
        var page = PaginatedResponse<ReceivableListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ReceivableListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ReceivableDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.Receivables.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ReceivableDetailResponse
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
            ? BaseResponse<ReceivableDetailResponse>.Failure("NotFound")
            : BaseResponse<ReceivableDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateReceivableRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Finance.Receivable
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
        _db.Receivables.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReceivableRequest request, CancellationToken ct = default)
    {
        var entity = await _db.Receivables.FirstOrDefaultAsync(e => e.Id == id, ct);
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
        var entity = await _db.Receivables.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
