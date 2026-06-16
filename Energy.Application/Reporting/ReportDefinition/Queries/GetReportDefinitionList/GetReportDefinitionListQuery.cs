using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using MediatR;

namespace Energy.Application.Reporting.ReportDefinition.Queries.GetReportDefinitionList;

/// <summary>Sayfalanmış ReportDefinition listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetReportDefinitionListQuery(GetReportDefinitionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>>;
