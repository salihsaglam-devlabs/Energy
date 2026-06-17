using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;

namespace Energy.Application.Contracts.Contract.Services;

/// <summary>Contract CRUD use-case sözleşmesi.</summary>
public interface IContractService
{
    /// <summary>Sayfalanmış Contract listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ContractListResponse>>> GetListAsync(GetContractListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ContractDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateContractRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
