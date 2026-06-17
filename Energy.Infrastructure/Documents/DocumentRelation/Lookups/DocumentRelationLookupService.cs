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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<DocumentRelationLookupResponse>)rows.Select(e => new DocumentRelationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.RelatedModule ?? "")) ? "Document Relation #" + e.Id.ToString().Substring(0, 8) : ((e.RelatedModule ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>.Success(items);
    }
}
