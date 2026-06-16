using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.AuditLog.Lookups;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;

namespace Energy.Infrastructure.Core.AuditLog.Lookups;

/// <summary>AuditLog lookup servisi (aktif + arama filtreli projection).</summary>
public class AuditLogLookupService : IAuditLogLookupService
{
    private readonly AppDbContext _db;

    public AuditLogLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.AuditLogs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.UserName.Contains(search));
        var items = await query
            .OrderBy(e => e.UserName)
            .Select(e => new AuditLogLookupResponse
            {
                Id = Guid.Empty,
                Code = null,
                Name = e.UserName,
                DisplayName = e.UserName,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<AuditLogLookupResponse>>.Success(items);
    }
}
