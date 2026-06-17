using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.FinancialTransaction.Lookups;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;

namespace Energy.Infrastructure.Finance.FinancialTransaction.Lookups;

/// <summary>FinancialTransaction lookup servisi (aktif + arama filtreli projection).</summary>
public class FinancialTransactionLookupService : IFinancialTransactionLookupService
{
    private readonly AppDbContext _db;

    public FinancialTransactionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.FinancialTransactions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<FinancialTransactionLookupResponse>)rows.Select(e => new FinancialTransactionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + (e.RelatedModule ?? "")) ? "Financial Transaction #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + (e.RelatedModule ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>.Success(items);
    }
}
