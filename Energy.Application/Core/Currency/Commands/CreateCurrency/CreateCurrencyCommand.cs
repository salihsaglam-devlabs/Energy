using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Currency.Requests;
using MediatR;

namespace Energy.Application.Core.Currency.Commands.CreateCurrency;

/// <summary>Yeni Currency oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateCurrencyCommand(CreateCurrencyRequest Request)
    : IRequest<BaseResponse<Guid>>;
