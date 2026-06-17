using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;

namespace Energy.Application.HR.TimesheetLine.Services;

/// <summary>TimesheetLine CRUD use-case sözleşmesi.</summary>
public interface ITimesheetLineService
{
    /// <summary>Sayfalanmış TimesheetLine listesi.</summary>
    Task<BaseResponse<PaginatedResponse<TimesheetLineListResponse>>> GetListAsync(GetTimesheetLineListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<TimesheetLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateTimesheetLineRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateTimesheetLineRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
