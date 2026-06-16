using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;

namespace Energy.Application.Catalog.MaterialUnitConversion.Services;

/// <summary>MaterialUnitConversion CRUD use-case sözleşmesi.</summary>
public interface IMaterialUnitConversionService
{
    /// <summary>Sayfalanmış MaterialUnitConversion listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>> GetListAsync(GetMaterialUnitConversionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MaterialUnitConversionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialUnitConversionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialUnitConversionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
