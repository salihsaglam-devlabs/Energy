using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Currency.Commands.DeleteCurrency;

/// <summary>Currency kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteCurrencyCommand(Guid Id) : IRequest<BaseResponse<bool>>;
