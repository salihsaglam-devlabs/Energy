using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.ExchangeRate.Queries.GetExchangeRateLookup;

/// <summary>ExchangeRate lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetExchangeRateLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>>;
