using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;

namespace Energy.Application.Contracts.ContractLine.Services;

/// <summary>ContractLine CRUD use-case sözleşmesi.</summary>
public interface IContractLineService
{
    /// <summary>Sayfalanmış ContractLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ContractLineListResponse>>> GetListAsync(GetContractLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ContractLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateContractLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateContractLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
