using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Documents.DocumentPermission.Lookups;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;

namespace Energy.Infrastructure.Documents.DocumentPermission.Lookups;

/// <summary>DocumentPermission lookup servisi (aktif + arama filtreli projection).</summary>
public class DocumentPermissionLookupService : IDocumentPermissionLookupService
{
    private readonly AppDbContext _db;

    public DocumentPermissionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DocumentPermissions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new DocumentPermissionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>.Success(items);
    }
}
