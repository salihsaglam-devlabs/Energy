using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Reporting.ReportDefinition.Lookups;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;

namespace Energy.Infrastructure.Reporting.ReportDefinition.Lookups;

/// <summary>ReportDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class ReportDefinitionLookupService : IReportDefinitionLookupService
{
    private readonly AppDbContext _db;

    public ReportDefinitionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ReportDefinitions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ReportDefinitionLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>.Success(items);
    }
}
