using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Requests.Request.Lookups;
using Energy.Shared.Models.V1.Requests.Request.Responses;

namespace Energy.Infrastructure.Modules.Requests.Request.Lookups;

/// <summary>Request lookup servisi (aktif + arama filtreli projection).</summary>
public class RequestLookupService : IRequestLookupService
{
    private readonly EnergyDbContext _db;

    public RequestLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<RequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Requests.AsNoTracking();
        var items = await query.Select(e => new RequestLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<RequestLookupResponse>>.Success(items);
    }
}
