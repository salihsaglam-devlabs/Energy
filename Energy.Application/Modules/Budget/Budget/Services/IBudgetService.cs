using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using Energy.Shared.Models.V1.Budget.Budget.Responses;

namespace Energy.Application.Modules.Budget.Budget.Services;

/// <summary>Budget CRUD use-case sözleşmesi.</summary>
public interface IBudgetService
{
    /// <summary>Sayfalanmış Budget listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BudgetListResponse>>> GetListAsync(GetBudgetListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BudgetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBudgetRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBudgetRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
