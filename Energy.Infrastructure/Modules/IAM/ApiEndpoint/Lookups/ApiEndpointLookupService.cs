using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.ApiEndpoint.Lookups;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

namespace Energy.Infrastructure.Modules.IAM.ApiEndpoint.Lookups;

/// <summary>ApiEndpoint lookup servisi (aktif + arama filtreli projection).</summary>
public class ApiEndpointLookupService : IApiEndpointLookupService
{
    private readonly EnergyDbContext _db;

    public ApiEndpointLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApiEndpointLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApiEndpoints.AsNoTracking();
        var items = await query.Select(e => new ApiEndpointLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApiEndpointLookupResponse>>.Success(items);
    }
}
