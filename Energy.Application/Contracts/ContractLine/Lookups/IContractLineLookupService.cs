using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;

namespace Energy.Application.Contracts.ContractLine.Lookups;

/// <summary>ContractLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IContractLineLookupService
{
    /// <summary>ContractLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ContractLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
