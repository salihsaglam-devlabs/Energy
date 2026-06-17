using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.FinancialTransactionLine.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;

namespace Energy.Infrastructure.Finance.FinancialTransactionLine.Lookups;

/// <summary>FinancialTransactionLine lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialTransactionLineLookupService : IFinancialTransactionLineLookupService
{
    private readonly AppDbContext _db;

    public FinancialTransactionLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactionLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<FinancialTransactionLineLookupResponse>)rows.Select(e => new FinancialTransactionLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.Amount.ToString()) ? "Financial Transaction Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.Amount.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>.Success(items);
    }
}
