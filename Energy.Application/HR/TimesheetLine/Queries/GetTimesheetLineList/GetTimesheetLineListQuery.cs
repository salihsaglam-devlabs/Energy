using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;
using MediatR;

namespace Energy.Application.HR.TimesheetLine.Queries.GetTimesheetLineList;

/// <summary>Sayfalanmış TimesheetLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetTimesheetLineListQuery(GetTimesheetLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<TimesheetLineListResponse>>>;
