using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Lookups;

/// <summary>BusinessPartnerContact lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBusinessPartnerContactLookupService
{
    /// <summary>BusinessPartnerContact lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BusinessPartnerContactLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
