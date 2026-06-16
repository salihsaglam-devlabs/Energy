using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentLookup;

/// <summary>WorkOrderAssignment lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWorkOrderAssignmentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>>;
