using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Responses;

namespace Energy.Application.BusinessPartners.BusinessPartnerAddress.Services;

/// <summary>BusinessPartnerAddress CRUD use-case sözleşmesi.</summary>
public interface IBusinessPartnerAddressService
{
    /// <summary>Sayfalanmış BusinessPartnerAddress listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BusinessPartnerAddressListResponse>>> GetListAsync(GetBusinessPartnerAddressListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BusinessPartnerAddressDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerAddressRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerAddressRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
