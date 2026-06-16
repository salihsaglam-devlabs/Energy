using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using MediatR;

namespace Energy.Application.HR.Timesheet.Commands.UpdateTimesheet;

/// <summary>Var olan Timesheet kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateTimesheetCommand(Guid Id, UpdateTimesheetRequest Request)
    : IRequest<BaseResponse<bool>>;
