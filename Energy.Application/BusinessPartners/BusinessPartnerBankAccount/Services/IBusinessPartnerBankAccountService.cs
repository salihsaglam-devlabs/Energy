using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Responses;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Services;

/// <summary>BusinessPartnerBankAccount CRUD use-case sözleşmesi.</summary>
public interface IBusinessPartnerBankAccountService
{
    /// <summary>Sayfalanmış BusinessPartnerBankAccount listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BusinessPartnerBankAccountListResponse>>> GetListAsync(GetBusinessPartnerBankAccountListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BusinessPartnerBankAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerBankAccountRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerBankAccountRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
