using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;
using MediatR;

namespace Energy.Application.Modules.HR.Timesheet.Queries.GetTimesheetList;

/// <summary>Sayfalanmış Timesheet listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetTimesheetListQuery(GetTimesheetListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<TimesheetListResponse>>>;
