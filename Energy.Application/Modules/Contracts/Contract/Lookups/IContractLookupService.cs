using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;

namespace Energy.Application.Modules.Contracts.Contract.Lookups;

/// <summary>Contract lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IContractLookupService
{
    /// <summary>Contract lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ContractLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
