using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceLookup;

/// <summary>EquipmentMaintenance lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetEquipmentMaintenanceLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>>;
