using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageLookup;

/// <summary>WorkOrderMaterialUsage lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWorkOrderMaterialUsageLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>>;
