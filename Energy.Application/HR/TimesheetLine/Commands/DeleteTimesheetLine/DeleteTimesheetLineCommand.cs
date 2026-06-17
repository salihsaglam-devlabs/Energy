using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.HR.TimesheetLine.Commands.DeleteTimesheetLine;

/// <summary>TimesheetLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteTimesheetLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
