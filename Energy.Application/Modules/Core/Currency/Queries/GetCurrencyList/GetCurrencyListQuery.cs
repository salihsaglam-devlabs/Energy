using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Requests;
using Energy.Shared.Models.V1.Core.Currency.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Currency.Queries.GetCurrencyList;

/// <summary>Sayfalanmış Currency listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetCurrencyListQuery(GetCurrencyListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<CurrencyListResponse>>>;
