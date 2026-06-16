using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Services;

/// <summary>MeasurementSheetLine CRUD use-case sözleşmesi.</summary>
public interface IMeasurementSheetLineService
{
    /// <summary>Sayfalanmış MeasurementSheetLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>> GetListAsync(GetMeasurementSheetLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<MeasurementSheetLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateMeasurementSheetLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMeasurementSheetLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
