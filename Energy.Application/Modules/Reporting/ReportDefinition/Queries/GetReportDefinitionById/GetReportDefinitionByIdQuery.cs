using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Queries.GetReportDefinitionById;

/// <summary>Kimliğe göre ReportDefinition detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetReportDefinitionByIdQuery(Guid Id)
    : IRequest<BaseResponse<ReportDefinitionDetailResponse>>;
