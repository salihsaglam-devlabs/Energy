using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Lookups;

/// <summary>BusinessPartner lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBusinessPartnerLookupService
{
    /// <summary>BusinessPartner lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BusinessPartnerLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
