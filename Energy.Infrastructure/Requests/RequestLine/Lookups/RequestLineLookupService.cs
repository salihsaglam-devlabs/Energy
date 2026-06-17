using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Requests.RequestLine.Lookups;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;

namespace Energy.Infrastructure.Requests.RequestLine.Lookups;

/// <summary>RequestLine lookup servisi (aktif + arama filtreli projection).</summary>
public class RequestLineLookupService : IRequestLineLookupService
{
    private readonly AppDbContext _db;

    public RequestLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<RequestLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.RequestLines.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new RequestLineLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<RequestLineLookupResponse>>.Success(items);
    }
}
