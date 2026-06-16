using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.IAM.ApiEndpoint.Lookups;
using Energy.Shared.Models.V1.IAM.ApiEndpoint.Responses;

namespace Energy.Infrastructure.Modules.IAM.ApiEndpoint.Lookups;

/// <summary>ApiEndpoint lookup servisi (aktif + arama filtreli projection).</summary>
public class ApiEndpointLookupService : IApiEndpointLookupService
{
    private readonly AppDbContext _db;

    public ApiEndpointLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApiEndpointLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApiEndpoints.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ApiEndpointLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Name,
                DisplayName = e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApiEndpointLookupResponse>>.Success(items);
    }
}
