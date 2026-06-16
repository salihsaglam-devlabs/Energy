using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Services;

/// <summary>ReportDefinition CRUD use-case sözleşmesi.</summary>
public interface IReportDefinitionService
{
    /// <summary>Sayfalanmış ReportDefinition listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>> GetListAsync(GetReportDefinitionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ReportDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateReportDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReportDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
