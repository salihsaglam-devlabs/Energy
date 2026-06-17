using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Requests;
using Energy.Shared.Models.V1.Core.Company.Responses;

namespace Energy.Application.Core.Company.Services;

/// <summary>Company CRUD use-case sözleşmesi.</summary>
public interface ICompanyService
{
    /// <summary>Sayfalanmış Company listesi.</summary>
    Task<BaseResponse<PaginatedResponse<CompanyListResponse>>> GetListAsync(GetCompanyListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<CompanyDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateCompanyRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateCompanyRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
