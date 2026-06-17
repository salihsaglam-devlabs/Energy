using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Contracts.ContractLine.Lookups;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;

namespace Energy.Infrastructure.Contracts.ContractLine.Lookups;

/// <summary>ContractLine lookup servisi (aktif + arama filtreli projection).</summary>
public class ContractLineLookupService : IContractLineLookupService
{
    private readonly AppDbContext _db;

    public ContractLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ContractLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ContractLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ContractLineLookupResponse>)rows.Select(e => new ContractLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.Quantity.ToString()) ? "Contract Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ContractLineLookupResponse>>.Success(items);
    }
}
