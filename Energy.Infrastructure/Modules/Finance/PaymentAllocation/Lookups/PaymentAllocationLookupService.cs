using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Finance.PaymentAllocation.Lookups;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;

namespace Energy.Infrastructure.Modules.Finance.PaymentAllocation.Lookups;

/// <summary>PaymentAllocation lookup servisi (aktif + arama filtreli projection).</summary>
public class PaymentAllocationLookupService : IPaymentAllocationLookupService
{
    private readonly EnergyDbContext _db;

    public PaymentAllocationLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.PaymentAllocations.AsNoTracking();
        var items = await query.Select(e => new PaymentAllocationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>.Success(items);
    }
}
