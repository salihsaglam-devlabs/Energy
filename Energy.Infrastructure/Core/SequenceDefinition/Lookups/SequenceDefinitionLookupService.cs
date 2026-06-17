using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.SequenceDefinition.Lookups;
using Energy.Shared.Models.V1.Core.SequenceDefinition.Responses;

namespace Energy.Infrastructure.Core.SequenceDefinition.Lookups;

/// <summary>SequenceDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class SequenceDefinitionLookupService : ISequenceDefinitionLookupService
{
    private readonly AppDbContext _db;

    public SequenceDefinitionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SequenceDefinitions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<SequenceDefinitionLookupResponse>)rows.Select(e => new SequenceDefinitionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Module ?? "") + " - " + e.NextNumber.ToString()) ? "Sequence Definition #" + e.Id.ToString().Substring(0, 8) : ((e.Module ?? "") + " - " + e.NextNumber.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<SequenceDefinitionLookupResponse>>.Success(items);
    }
}
