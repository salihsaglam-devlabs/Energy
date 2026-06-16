using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;

namespace Energy.Application.Modules.Finance.FinancialAccount.Services;

/// <summary>FinancialAccount CRUD use-case sözleşmesi.</summary>
public interface IFinancialAccountService
{
    /// <summary>Sayfalanmış FinancialAccount listesi.</summary>
    Task<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>> GetListAsync(GetFinancialAccountListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<FinancialAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateFinancialAccountRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialAccountRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
