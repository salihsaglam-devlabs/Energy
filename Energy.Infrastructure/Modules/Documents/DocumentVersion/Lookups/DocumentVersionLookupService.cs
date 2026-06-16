using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.DocumentVersion.Lookups;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

namespace Energy.Infrastructure.Modules.Documents.DocumentVersion.Lookups;

/// <summary>DocumentVersion lookup servisi (aktif + arama filtreli projection).</summary>
public class DocumentVersionLookupService : IDocumentVersionLookupService
{
    private readonly AppDbContext _db;

    public DocumentVersionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DocumentVersions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new DocumentVersionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>.Success(items);
    }
}
