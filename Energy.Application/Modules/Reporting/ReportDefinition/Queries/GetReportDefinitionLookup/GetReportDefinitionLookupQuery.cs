using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Queries.GetReportDefinitionLookup;

/// <summary>ReportDefinition lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetReportDefinitionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>>;
