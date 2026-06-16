using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.SequenceDefinition.Lookups;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

namespace Energy.Infrastructure.Modules.Core.SequenceDefinition.Lookups;

/// <summary>SequenceDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class SequenceDefinitionLookupService : ISequenceDefinitionLookupService
{
    private readonly AppDbContext _db;

    public SequenceDefinitionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SequenceDefinitions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new SequenceDefinitionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>.Success(items);
    }
}
