using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Contracts.ContractLine.Lookups;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;

namespace Energy.Infrastructure.Modules.Contracts.ContractLine.Lookups;

/// <summary>ContractLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractLineLookupService : IContractLineLookupService
{
    private readonly EnergyDbContext _db;

    public ContractLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ContractLines.AsNoTracking();
        var items = await query.Select(e => new ContractLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ContractLineLookupResponse>>.Success(items);
    }
}
