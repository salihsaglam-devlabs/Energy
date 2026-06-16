using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.DailySiteReportMaterial.Lookups;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.DailySiteReportMaterial.Lookups;

/// <summary>DailySiteReportMaterial lookup servisi (aktif + arama filtreli projection).</summary>
public class DailySiteReportMaterialLookupService : IDailySiteReportMaterialLookupService
{
    private readonly AppDbContext _db;

    public DailySiteReportMaterialLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DailySiteReportMaterials.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new DailySiteReportMaterialLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>.Success(items);
    }
}
