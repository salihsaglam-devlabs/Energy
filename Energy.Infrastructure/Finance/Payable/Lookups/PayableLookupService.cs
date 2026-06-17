using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.Payable.Lookups;
using Energy.Shared.Models.V1.Finance.Payable.Responses;

namespace Energy.Infrastructure.Finance.Payable.Lookups;

/// <summary>Payable lookup servisi (aktif + arama filtreli projection).</summary>
public class PayableLookupService : IPayableLookupService
{
    private readonly AppDbContext _db;

    public PayableLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PayableLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Payables.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<PayableLookupResponse>)rows.Select(e => new PayableLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.RelatedModule ?? "") + " - " + e.DueDate.ToString("yyyy-MM-dd")) ? "Payable #" + e.Id.ToString().Substring(0, 8) : ((e.RelatedModule ?? "") + " - " + e.DueDate.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<PayableLookupResponse>>.Success(items);
    }
}
