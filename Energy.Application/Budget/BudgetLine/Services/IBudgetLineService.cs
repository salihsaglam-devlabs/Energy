using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;

namespace Energy.Application.Budget.BudgetLine.Services;

/// <summary>BudgetLine CRUD use-case sözleşmesi.</summary>
public interface IBudgetLineService
{
    /// <summary>Sayfalanmış BudgetLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BudgetLineListResponse>>> GetListAsync(GetBudgetLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BudgetLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBudgetLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBudgetLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
