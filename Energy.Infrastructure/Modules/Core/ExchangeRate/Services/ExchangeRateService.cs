using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.ExchangeRate.Services;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;

namespace Energy.Infrastructure.Modules.Core.ExchangeRate.Services;

/// <summary>ExchangeRate CRUD servisi (projection, pagination, soft-delete).</summary>
public class ExchangeRateService : IExchangeRateService
{
    private readonly AppDbContext _db;

    public ExchangeRateService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>> GetListAsync(GetExchangeRateListRequest request, CancellationToken ct = default)
    {
        var query = _db.ExchangeRates.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(e => e.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ExchangeRateListResponse
            {
                Id = e.Id,
                CurrencyId = e.CurrencyId,
                RateDate = e.RateDate,
                Rate = e.Rate,
                CreatedAt = e.CreatedAt
            })
            .ToListAsync(ct);
        var page = PaginatedResponse<ExchangeRateListResponse>.Create(items, request.PageNumber, request.PageSize, total);
        return BaseResponse<PaginatedResponse<ExchangeRateListResponse>>.Success(page);
    }

    public async Task<BaseResponse<ExchangeRateDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dto = await _db.ExchangeRates.AsNoTracking().Where(e => e.Id == id)
            .Select(e => new ExchangeRateDetailResponse
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy,
                UpdatedAt = e.UpdatedAt,
                UpdatedBy = e.UpdatedBy,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                DeletedBy = e.DeletedBy,
                CurrencyId = e.CurrencyId,
                RateDate = e.RateDate,
                Rate = e.Rate
            }).FirstOrDefaultAsync(ct);
        return dto is null
            ? BaseResponse<ExchangeRateDetailResponse>.Failure("NotFound")
            : BaseResponse<ExchangeRateDetailResponse>.Success(dto);
    }

    public async Task<BaseResponse<Guid>> CreateAsync(CreateExchangeRateRequest request, CancellationToken ct = default)
    {
        var entity = new global::Energy.Domain.Modules.Core.ExchangeRate
        {
            Id = Guid.NewGuid(),
            CurrencyId = request.CurrencyId,
            RateDate = request.RateDate,
            Rate = request.Rate,
            CreatedAt = DateTime.UtcNow,
        };
        _db.ExchangeRates.Add(entity);
        await _db.SaveChangesAsync(ct);
        return BaseResponse<Guid>.Success(entity.Id, "Created");
    }

    public async Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExchangeRateRequest request, CancellationToken ct = default)
    {
        var entity = await _db.ExchangeRates.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
            entity.CurrencyId = request.CurrencyId;
            entity.RateDate = request.RateDate;
            entity.Rate = request.Rate;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Updated");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.ExchangeRates.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null) return BaseResponse<bool>.Failure("NotFound");
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return BaseResponse<bool>.Success(true, "Deleted");
    }
}
