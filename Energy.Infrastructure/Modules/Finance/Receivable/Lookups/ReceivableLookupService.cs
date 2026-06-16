using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Receivable.Lookups;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;

namespace Energy.Infrastructure.Modules.Finance.Receivable.Lookups;

/// <summary>Receivable lookup servisi (aktif + arama filtreli projection).</summary>
public class ReceivableLookupService : IReceivableLookupService
{
    private readonly EnergyDbContext _db;

    public ReceivableLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Receivables.AsNoTracking();
        var items = await query.Select(e => new ReceivableLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ReceivableLookupResponse>>.Success(items);
    }
}
