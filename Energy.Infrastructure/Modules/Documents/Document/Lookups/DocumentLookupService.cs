using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Documents.Document.Lookups;
using Energy.Shared.Models.V1.Documents.Document.Responses;

namespace Energy.Infrastructure.Modules.Documents.Document.Lookups;

/// <summary>Document lookup servisi (aktif + arama filtreli projection).</summary>
public class DocumentLookupService : IDocumentLookupService
{
    private readonly EnergyDbContext _db;

    public DocumentLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DocumentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Documents.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new DocumentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DocumentLookupResponse>>.Success(items);
    }
}
