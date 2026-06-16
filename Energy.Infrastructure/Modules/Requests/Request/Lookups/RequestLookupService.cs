using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Requests.Request.Lookups;
using Energy.Shared.Models.V1.Requests.Request.Responses;

namespace Energy.Infrastructure.Modules.Requests.Request.Lookups;

/// <summary>Request lookup servisi (aktif + arama filtreli projection).</summary>
public class RequestLookupService : IRequestLookupService
{
    private readonly AppDbContext _db;

    public RequestLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<RequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Requests.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.RequestNo.Contains(search));
        var items = await query
            .OrderBy(e => e.RequestNo)
            .Select(e => new RequestLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.RequestNo,
                DisplayName = e.RequestNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<RequestLookupResponse>>.Success(items);
    }
}
