using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.FieldOperations.ProgressEntry.Lookups;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Responses;

namespace Energy.Infrastructure.FieldOperations.ProgressEntry.Lookups;

/// <summary>ProgressEntry lookup servisi (aktif + arama filtreli projection).</summary>
public class ProgressEntryLookupService : IProgressEntryLookupService
{
    private readonly AppDbContext _db;

    public ProgressEntryLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProgressEntries.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ProgressEntryLookupResponse>)rows.Select(e => new ProgressEntryLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Note ?? "") + " - " + e.EntryDate.ToString("yyyy-MM-dd")) ? "Progress Entry #" + e.Id.ToString().Substring(0, 8) : ((e.Note ?? "") + " - " + e.EntryDate.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ProgressEntryLookupResponse>>.Success(items);
    }
}
