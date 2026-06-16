using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Services;

/// <summary>DashboardWidget CRUD use-case sözleşmesi.</summary>
public interface IDashboardWidgetService
{
    /// <summary>Sayfalanmış DashboardWidget listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DashboardWidgetListResponse>>> GetListAsync(GetDashboardWidgetListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DashboardWidgetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDashboardWidgetRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDashboardWidgetRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
