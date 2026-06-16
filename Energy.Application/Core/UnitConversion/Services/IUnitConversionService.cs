using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitConversion.Requests;
using Energy.Shared.Models.V1.Core.UnitConversion.Responses;

namespace Energy.Application.Core.UnitConversion.Services;

/// <summary>UnitConversion CRUD use-case sözleşmesi.</summary>
public interface IUnitConversionService
{
    /// <summary>Sayfalanmış UnitConversion listesi.</summary>
    Task<BaseResponse<PaginatedResponse<UnitConversionListResponse>>> GetListAsync(GetUnitConversionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<UnitConversionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateUnitConversionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUnitConversionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
