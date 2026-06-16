using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemLookup;

/// <summary>WorkOrderChecklistItem lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWorkOrderChecklistItemLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>>;
