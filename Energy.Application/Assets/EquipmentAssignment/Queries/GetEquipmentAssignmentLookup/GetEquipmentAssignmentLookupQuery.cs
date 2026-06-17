using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Queries.GetEquipmentAssignmentLookup;

/// <summary>EquipmentAssignment lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetEquipmentAssignmentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>>;
