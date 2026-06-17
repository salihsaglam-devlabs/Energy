using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Requests;
using MediatR;

namespace Energy.Application.Core.Currency.Commands.UpdateCurrency;

/// <summary>Var olan Currency kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateCurrencyCommand(Guid Id, UpdateCurrencyRequest Request)
    : IRequest<BaseResponse<bool>>;
