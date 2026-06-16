using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Commands.UpdateTimesheetLine;

/// <summary>Var olan TimesheetLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateTimesheetLineCommand(Guid Id, UpdateTimesheetLineRequest Request)
    : IRequest<BaseResponse<bool>>;
