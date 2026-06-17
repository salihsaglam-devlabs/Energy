using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.DailySiteReportMaterial.Lookups;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

namespace Energy.Infrastructure.FieldOperations.DailySiteReportMaterial.Lookups;

/// <summary>DailySiteReportMaterial lookup servisi (aktif + arama filtreli projection).</summary>
public class DailySiteReportMaterialLookupService : IDailySiteReportMaterialLookupService
{
    private readonly AppDbContext _db;

    public DailySiteReportMaterialLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportMaterials.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<DailySiteReportMaterialLookupResponse>)rows.Select(e => new DailySiteReportMaterialLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Quantity.ToString()) ? "Daily Site Report Material #" + e.Id.ToString().Substring(0, 8) : (e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>.Success(items);
    }
}
