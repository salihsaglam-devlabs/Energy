using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using MediatR;

namespace Energy.Application.HR.Timesheet.Commands.CreateTimesheet;

/// <summary>Yeni Timesheet oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateTimesheetCommand(CreateTimesheetRequest Request)
    : IRequest<BaseResponse<Guid>>;
