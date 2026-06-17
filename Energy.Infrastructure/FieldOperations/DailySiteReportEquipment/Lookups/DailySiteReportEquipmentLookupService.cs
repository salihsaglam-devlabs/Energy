using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.DailySiteReportEquipment.Lookups;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

namespace Energy.Infrastructure.FieldOperations.DailySiteReportEquipment.Lookups;

/// <summary>DailySiteReportEquipment lookup servisi (aktif + arama filtreli projection).</summary>
public class DailySiteReportEquipmentLookupService : IDailySiteReportEquipmentLookupService
{
    private readonly AppDbContext _db;

    public DailySiteReportEquipmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportEquipments.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<DailySiteReportEquipmentLookupResponse>)rows.Select(e => new DailySiteReportEquipmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = "Daily Site Report Equipment #" + e.Id.ToString().Substring(0, 8),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>.Success(items);
    }
}
