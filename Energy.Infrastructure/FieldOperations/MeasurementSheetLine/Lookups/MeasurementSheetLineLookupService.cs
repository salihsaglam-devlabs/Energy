using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.MeasurementSheetLine.Lookups;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

namespace Energy.Infrastructure.FieldOperations.MeasurementSheetLine.Lookups;

/// <summary>MeasurementSheetLine lookup servisi (aktif + arama filtreli projection).</summary>
public class MeasurementSheetLineLookupService : IMeasurementSheetLineLookupService
{
    private readonly AppDbContext _db;

    public MeasurementSheetLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.MeasurementSheetLines.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new MeasurementSheetLineLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>.Success(items);
    }
}
