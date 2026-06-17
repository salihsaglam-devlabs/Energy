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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<MeasurementSheetLineLookupResponse>)rows.Select(e => new MeasurementSheetLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.Quantity.ToString()) ? "Measurement Sheet Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>.Success(items);
    }
}
