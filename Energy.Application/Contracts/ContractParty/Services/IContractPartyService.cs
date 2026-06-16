using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;

namespace Energy.Application.Contracts.ContractParty.Services;

/// <summary>ContractParty CRUD use-case sözleşmesi.</summary>
public interface IContractPartyService
{
    /// <summary>Sayfalanmış ContractParty listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ContractPartyListResponse>>> GetListAsync(GetContractPartyListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ContractPartyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateContractPartyRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractPartyRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
