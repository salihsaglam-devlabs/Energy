using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;

namespace Energy.Application.Catalog.Brand.Services;

/// <summary>Brand CRUD use-case sözleşmesi.</summary>
public interface IBrandService
{
    /// <summary>Sayfalanmış Brand listesi.</summary>
    Task<BaseResponse<PaginatedResponse<BrandListResponse>>> GetListAsync(GetBrandListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<BrandDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateBrandRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateBrandRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
