using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerLookup;

/// <summary>DailySiteReportWorker lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDailySiteReportWorkerLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>>;
