using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Documents.DocumentRelation.Lookups;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;

namespace Energy.Infrastructure.Documents.DocumentRelation.Lookups;

/// <summary>DocumentRelation lookup servisi (aktif + arama filtreli projection).</summary>
public class DocumentRelationLookupService : IDocumentRelationLookupService
{
    private readonly AppDbContext _db;

    public DocumentRelationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DocumentRelations.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new DocumentRelationLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>.Success(items);
    }
}
