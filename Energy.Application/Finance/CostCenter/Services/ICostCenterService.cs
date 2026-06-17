using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;

namespace Energy.Application.Finance.CostCenter.Services;

/// <summary>CostCenter CRUD use-case sözleşmesi.</summary>
public interface ICostCenterService
{
    /// <summary>Sayfalanmış CostCenter listesi.</summary>
    Task<BaseResponse<PaginatedResponse<CostCenterListResponse>>> GetListAsync(GetCostCenterListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<CostCenterDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateCostCenterRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCostCenterRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
