using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using MediatR;

namespace Energy.Application.HR.Timesheet.Queries.GetTimesheetById;

/// <summary>Kimliğe göre Timesheet detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetTimesheetByIdQuery(Guid Id)
    : IRequest<BaseResponse<TimesheetDetailResponse>>;
