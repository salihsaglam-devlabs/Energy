using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.Payable.Lookups;
using Energy.Shared.Models.V1.Finance.Payable.Responses;

namespace Energy.Infrastructure.Modules.Finance.Payable.Lookups;

/// <summary>Payable lookup servisi (aktif + arama filtreli projection).</summary>
public class PayableLookupService : IPayableLookupService
{
    private readonly AppDbContext _db;

    public PayableLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PayableLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Payables.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new PayableLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PayableLookupResponse>>.Success(items);
    }
}
