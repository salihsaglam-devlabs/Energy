using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.MeasurementSheet.Lookups;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.MeasurementSheet.Lookups;

/// <summary>MeasurementSheet lookup servisi (aktif + arama filtreli projection).</summary>
public class MeasurementSheetLookupService : IMeasurementSheetLookupService
{
    private readonly AppDbContext _db;

    public MeasurementSheetLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MeasurementSheets.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.SheetNo.Contains(search));
        var items = await query
            .OrderBy(e => e.SheetNo)
            .Select(e => new MeasurementSheetLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.SheetNo,
                DisplayName = e.SheetNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MeasurementSheetLookupResponse>>.Success(items);
    }
}
