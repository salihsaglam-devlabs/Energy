using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.TimesheetLine.Queries.GetTimesheetLineById;

/// <summary>Kimliğe göre TimesheetLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetTimesheetLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<TimesheetLineDetailResponse>>;
