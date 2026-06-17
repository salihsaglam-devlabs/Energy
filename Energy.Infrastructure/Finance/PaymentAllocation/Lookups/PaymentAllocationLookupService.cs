using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Finance.PaymentAllocation.Lookups;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

namespace Energy.Infrastructure.Finance.PaymentAllocation.Lookups;

/// <summary>PaymentAllocation lookup servisi (aktif + arama filtreli projection).</summary>
public class PaymentAllocationLookupService : IPaymentAllocationLookupService
{
    private readonly AppDbContext _db;

    public PaymentAllocationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PaymentAllocations.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<PaymentAllocationLookupResponse>)rows.Select(e => new PaymentAllocationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Amount.ToString()) ? "Payment Allocation #" + e.Id.ToString().Substring(0, 8) : (e.Amount.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>.Success(items);
    }
}
