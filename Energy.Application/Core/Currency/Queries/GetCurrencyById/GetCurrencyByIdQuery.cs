using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Responses;
using MediatR;

namespace Energy.Application.Core.Currency.Queries.GetCurrencyById;

/// <summary>Kimliğe göre Currency detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetCurrencyByIdQuery(Guid Id)
    : IRequest<BaseResponse<CurrencyDetailResponse>>;
