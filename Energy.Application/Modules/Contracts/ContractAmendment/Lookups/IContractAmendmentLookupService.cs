using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Lookups;

/// <summary>ContractAmendment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IContractAmendmentLookupService
{
    /// <summary>ContractAmendment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
