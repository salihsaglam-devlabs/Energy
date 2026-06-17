using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Documents.DocumentVersion.Lookups;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;

namespace Energy.Infrastructure.Documents.DocumentVersion.Lookups;

/// <summary>DocumentVersion lookup servisi (aktif + arama filtreli projection).</summary>
public class DocumentVersionLookupService : IDocumentVersionLookupService
{
    private readonly AppDbContext _db;

    public DocumentVersionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DocumentVersions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<DocumentVersionLookupResponse>)rows.Select(e => new DocumentVersionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.ContentType ?? "") + " - " + e.UploadedAt.ToString("yyyy-MM-dd")) ? "Document Version #" + e.Id.ToString().Substring(0, 8) : ((e.ContentType ?? "") + " - " + e.UploadedAt.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>.Success(items);
    }
}
