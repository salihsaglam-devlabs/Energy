using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Responses;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Services;

/// <summary>BusinessPartner CRUD use-case sözleşmesi.</summary>
public interface IBusinessPartnerService
{
    /// <summary>Sayfalanmış BusinessPartner listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BusinessPartnerListResponse>>> GetListAsync(GetBusinessPartnerListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BusinessPartnerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBusinessPartnerRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBusinessPartnerRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
