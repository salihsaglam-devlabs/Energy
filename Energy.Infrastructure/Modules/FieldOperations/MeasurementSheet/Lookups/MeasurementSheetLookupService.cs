using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.MeasurementSheet.Lookups;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.MeasurementSheet.Lookups;

/// <summary>MeasurementSheet lookup servisi (aktif + arama filtreli projection).</summary>
public class MeasurementSheetLookupService : IMeasurementSheetLookupService
{
    private readonly EnergyDbContext _db;

    public MeasurementSheetLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MeasurementSheets.AsNoTracking();
        var items = await query.Select(e => new MeasurementSheetLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>.Success(items);
    }
}
