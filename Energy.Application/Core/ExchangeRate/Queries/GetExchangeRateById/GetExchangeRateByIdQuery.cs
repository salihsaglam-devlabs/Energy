using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Queries.GetExchangeRateById;

/// <summary>Kimliğe göre ExchangeRate detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetExchangeRateByIdQuery(Guid Id)
    : IRequest<BaseResponse<ExchangeRateDetailResponse>>;
