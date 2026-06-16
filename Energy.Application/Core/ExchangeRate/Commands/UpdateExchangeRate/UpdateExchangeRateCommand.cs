using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Commands.UpdateExchangeRate;

/// <summary>Var olan ExchangeRate kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateExchangeRateCommand(Guid Id, UpdateExchangeRateRequest Request)
    : IRequest<BaseResponse<bool>>;
