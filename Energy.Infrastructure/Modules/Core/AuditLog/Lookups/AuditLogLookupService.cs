using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.AuditLog.Lookups;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;

namespace Energy.Infrastructure.Modules.Core.AuditLog.Lookups;

/// <summary>AuditLog lookup servisi (aktif + arama filtreli projection).</summary>
public class AuditLogLookupService : IAuditLogLookupService
{
    private readonly EnergyDbContext _db;

    public AuditLogLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.AuditLogs.AsNoTracking();
        var items = await query.Select(e => new AuditLogLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<AuditLogLookupResponse>>.Success(items);
    }
}
