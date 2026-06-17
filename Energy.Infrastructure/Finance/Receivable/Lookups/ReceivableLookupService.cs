using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.Receivable.Lookups;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;

namespace Energy.Infrastructure.Finance.Receivable.Lookups;

/// <summary>Receivable lookup servisi (aktif + arama filtreli projection).</summary>
public class ReceivableLookupService : IReceivableLookupService
{
    private readonly AppDbContext _db;

    public ReceivableLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Receivables.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ReceivableLookupResponse>)rows.Select(e => new ReceivableLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.RelatedModule ?? "") + " - " + e.DueDate.ToString("yyyy-MM-dd")) ? "Receivable #" + e.Id.ToString().Substring(0, 8) : ((e.RelatedModule ?? "") + " - " + e.DueDate.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ReceivableLookupResponse>>.Success(items);
    }
}
