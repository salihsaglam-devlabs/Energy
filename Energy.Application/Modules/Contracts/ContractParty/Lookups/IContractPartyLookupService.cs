using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

namespace Energy.Application.Modules.Contracts.ContractParty.Lookups;

/// <summary>ContractParty lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IContractPartyLookupService
{
    /// <summary>ContractParty lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
