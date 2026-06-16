using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Requests.RequestType.Lookups;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;

namespace Energy.Infrastructure.Modules.Requests.RequestType.Lookups;

/// <summary>RequestType lookup servisi (aktif + arama filtreli projection).</summary>
public class RequestTypeLookupService : IRequestTypeLookupService
{
    private readonly EnergyDbContext _db;

    public RequestTypeLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.RequestTypes.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new RequestTypeLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<RequestTypeLookupResponse>>.Success(items);
    }
}
