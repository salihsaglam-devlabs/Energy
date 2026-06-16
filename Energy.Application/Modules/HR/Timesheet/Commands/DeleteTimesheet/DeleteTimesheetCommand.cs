using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.Timesheet.Commands.DeleteTimesheet;

/// <summary>Timesheet kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteTimesheetCommand(Guid Id) : IRequest<BaseResponse<bool>>;
