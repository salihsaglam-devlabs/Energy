using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Commands.DeleteExchangeRate;

/// <summary>ExchangeRate kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteExchangeRateCommand(Guid Id) : IRequest<BaseResponse<bool>>;
