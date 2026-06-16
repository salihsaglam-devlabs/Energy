using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryLookup;

/// <summary>WorkOrderStatusHistory lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWorkOrderStatusHistoryLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>>;
