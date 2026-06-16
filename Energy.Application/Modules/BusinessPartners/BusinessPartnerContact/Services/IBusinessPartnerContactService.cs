using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Responses;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Services;

/// <summary>BusinessPartnerContact CRUD use-case sözleşmesi.</summary>
public interface IBusinessPartnerContactService
{
    /// <summary>Sayfalanmış BusinessPartnerContact listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BusinessPartnerContactListResponse>>> GetListAsync(GetBusinessPartnerContactListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BusinessPartnerContactDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerContactRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerContactRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
