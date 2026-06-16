using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;

namespace Energy.Application.Modules.HR.Timesheet.Services;

/// <summary>Timesheet CRUD use-case sözleşmesi.</summary>
public interface ITimesheetService
{
    /// <summary>Sayfalanmış Timesheet listesi.</summary>
    Task<BaseResponse<PaginatedResponse<TimesheetListResponse>>> GetListAsync(GetTimesheetListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<TimesheetDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateTimesheetRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateTimesheetRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
