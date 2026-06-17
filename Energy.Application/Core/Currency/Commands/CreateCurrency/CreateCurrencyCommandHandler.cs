using Energy.Application.Core.Currency.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Currency.Commands.CreateCurrency;

/// <summary>
/// <see cref="CreateCurrencyCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ICurrencyService"/>'i orkestre eder.
/// </summary>
public sealed class CreateCurrencyCommandHandler
    : IRequestHandler<CreateCurrencyCommand, BaseResponse<Guid>>
{
    private readonly ICurrencyService _service;

    public CreateCurrencyCommandHandler(ICurrencyService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateCurrencyCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
