using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.UnitOfMeasure.Lookups;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;

namespace Energy.Infrastructure.Modules.Core.UnitOfMeasure.Lookups;

/// <summary>UnitOfMeasure lookup servisi (aktif + arama filtreli projection).</summary>
public class UnitOfMeasureLookupService : IUnitOfMeasureLookupService
{
    private readonly EnergyDbContext _db;

    public UnitOfMeasureLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.UnitsOfMeasure.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new UnitOfMeasureLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>.Success(items);
    }
}
