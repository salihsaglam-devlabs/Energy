using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.UnitOfMeasure.Lookups;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;

namespace Energy.Infrastructure.Core.UnitOfMeasure.Lookups;

/// <summary>UnitOfMeasure lookup servisi (aktif + arama filtreli projection).</summary>
public class UnitOfMeasureLookupService : IUnitOfMeasureLookupService
{
    private readonly AppDbContext _db;

    public UnitOfMeasureLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UnitsOfMeasure.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new UnitOfMeasureLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>.Success(items);
    }
}
