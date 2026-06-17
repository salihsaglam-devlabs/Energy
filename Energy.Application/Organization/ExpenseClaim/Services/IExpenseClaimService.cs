using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;

namespace Energy.Application.Organization.ExpenseClaim.Services;

/// <summary>ExpenseClaim CRUD use-case sözleşmesi.</summary>
public interface IExpenseClaimService
{
    /// <summary>Sayfalanmış ExpenseClaim listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>> GetListAsync(GetExpenseClaimListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ExpenseClaimDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateExpenseClaimRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExpenseClaimRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
