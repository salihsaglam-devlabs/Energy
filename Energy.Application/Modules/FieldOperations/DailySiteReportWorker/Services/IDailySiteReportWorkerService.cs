using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Services;

/// <summary>DailySiteReportWorker CRUD use-case sözleşmesi.</summary>
public interface IDailySiteReportWorkerService
{
    /// <summary>Sayfalanmış DailySiteReportWorker listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DailySiteReportWorkerListResponse>>> GetListAsync(GetDailySiteReportWorkerListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DailySiteReportWorkerDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDailySiteReportWorkerRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDailySiteReportWorkerRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
