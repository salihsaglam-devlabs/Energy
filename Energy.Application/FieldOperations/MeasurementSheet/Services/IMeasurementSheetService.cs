using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;

namespace Energy.Application.FieldOperations.MeasurementSheet.Services;

/// <summary>MeasurementSheet CRUD use-case sözleşmesi.</summary>
public interface IMeasurementSheetService
{
    /// <summary>Sayfalanmış MeasurementSheet listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MeasurementSheetListResponse>>> GetListAsync(GetMeasurementSheetListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MeasurementSheetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMeasurementSheetRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMeasurementSheetRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
