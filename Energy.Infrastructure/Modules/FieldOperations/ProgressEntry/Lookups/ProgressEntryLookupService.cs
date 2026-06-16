using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.FieldOperations.ProgressEntry.Lookups;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

namespace Energy.Infrastructure.Modules.FieldOperations.ProgressEntry.Lookups;

/// <summary>ProgressEntry lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressEntryLookupService : IProgressEntryLookupService
{
    private readonly AppDbContext _db;

    public ProgressEntryLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressEntries.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ProgressEntryLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>.Success(items);
    }
}
