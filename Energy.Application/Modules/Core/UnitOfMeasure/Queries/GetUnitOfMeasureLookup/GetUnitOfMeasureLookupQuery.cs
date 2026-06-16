using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Queries.GetUnitOfMeasureLookup;

/// <summary>UnitOfMeasure lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetUnitOfMeasureLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>>;
