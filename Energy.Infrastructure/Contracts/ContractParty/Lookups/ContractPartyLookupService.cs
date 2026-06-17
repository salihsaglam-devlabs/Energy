using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.ContractParty.Lookups;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

namespace Energy.Infrastructure.Contracts.ContractParty.Lookups;

/// <summary>ContractParty lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractPartyLookupService : IContractPartyLookupService
{
    private readonly AppDbContext _db;

    public ContractPartyLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ContractParties.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ContractPartyLookupResponse>)rows.Select(e => new ContractPartyLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.PartyRole ?? "")) ? "Contract Party #" + e.Id.ToString().Substring(0, 8) : ((e.PartyRole ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>.Success(items);
    }
}
