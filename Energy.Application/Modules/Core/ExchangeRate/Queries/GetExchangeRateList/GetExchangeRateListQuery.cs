using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.ExchangeRate.Queries.GetExchangeRateList;

/// <summary>Sayfalanmış ExchangeRate listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetExchangeRateListQuery(GetExchangeRateListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>>;
