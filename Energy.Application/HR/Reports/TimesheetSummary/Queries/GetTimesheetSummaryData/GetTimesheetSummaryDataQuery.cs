using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Requests;
using Energy.Shared.Models.V1.HR.Reports.TimesheetSummary.Responses;
using MediatR;

namespace Energy.Application.HR.Reports.TimesheetSummary.Queries.GetTimesheetSummaryData;

/// <summary>TimesheetSummary rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetTimesheetSummaryDataQuery(TimesheetSummaryRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<TimesheetSummaryRowResponse>>>;
