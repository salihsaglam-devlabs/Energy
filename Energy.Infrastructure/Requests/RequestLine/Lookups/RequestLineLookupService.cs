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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<RequestLineLookupResponse>)rows.Select(e => new RequestLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Note ?? "") + " - " + e.Quantity.ToString()) ? "Request Line #" + e.Id.ToString().Substring(0, 8) : ((e.Note ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<RequestLineLookupResponse>>.Success(items);
    }
}
