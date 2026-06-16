using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Lookups;

/// <summary>BusinessPartnerAddress lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBusinessPartnerAddressLookupService
{
    /// <summary>BusinessPartnerAddress lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BusinessPartnerAddressLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
