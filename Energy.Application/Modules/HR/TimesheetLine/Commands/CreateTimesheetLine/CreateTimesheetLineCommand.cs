using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Commands.CreateTimesheetLine;

/// <summary>Yeni TimesheetLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateTimesheetLineCommand(CreateTimesheetLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
