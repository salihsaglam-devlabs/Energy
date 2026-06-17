using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

namespace Energy.Application.FieldOperations.DailySiteReport.Services;

/// <summary>DailySiteReport CRUD use-case sözleşmesi.</summary>
public interface IDailySiteReportService
{
    /// <summary>Sayfalanmış DailySiteReport listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DailySiteReportListResponse>>> GetListAsync(GetDailySiteReportListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DailySiteReportDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
