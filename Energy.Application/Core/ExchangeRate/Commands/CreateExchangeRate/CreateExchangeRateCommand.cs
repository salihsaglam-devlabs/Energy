using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Commands.CreateExchangeRate;

/// <summary>Yeni ExchangeRate oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateExchangeRateCommand(CreateExchangeRateRequest Request)
    : IRequest<BaseResponse<Guid>>;
