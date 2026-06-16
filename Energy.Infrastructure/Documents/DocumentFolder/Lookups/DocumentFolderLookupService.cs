using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Documents.DocumentFolder.Lookups;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;

namespace Energy.Infrastructure.Documents.DocumentFolder.Lookups;

/// <summary>DocumentFolder lookup servisi (aktif + arama filtreli projection).</summary>
public class DocumentFolderLookupService : IDocumentFolderLookupService
{
    private readonly AppDbContext _db;

    public DocumentFolderLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<DocumentFolderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.DocumentFolders.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new DocumentFolderLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Name,
                DisplayName = e.Name,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<DocumentFolderLookupResponse>>.Success(items);
    }
}
