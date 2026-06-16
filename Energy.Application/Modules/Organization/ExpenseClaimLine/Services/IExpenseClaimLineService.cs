using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Services;

/// <summary>ExpenseClaimLine CRUD use-case sözleşmesi.</summary>
public interface IExpenseClaimLineService
{
    /// <summary>Sayfalanmış ExpenseClaimLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>> GetListAsync(GetExpenseClaimLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ExpenseClaimLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateExpenseClaimLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExpenseClaimLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
