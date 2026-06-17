using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Lookups;

/// <summary>BusinessPartnerBankAccount lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBusinessPartnerBankAccountLookupService
{
    /// <summary>BusinessPartnerBankAccount lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BusinessPartnerBankAccountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
