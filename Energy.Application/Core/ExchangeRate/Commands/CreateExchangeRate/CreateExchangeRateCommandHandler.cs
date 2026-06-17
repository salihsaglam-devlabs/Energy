using Energy.Application.Core.ExchangeRate.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Commands.CreateExchangeRate;

/// <summary>
/// <see cref="CreateExchangeRateCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IExchangeRateService"/>'i orkestre eder.
/// </summary>
public sealed class CreateExchangeRateCommandHandler
    : IRequestHandler<CreateExchangeRateCommand, BaseResponse<Guid>>
{
    private readonly IExchangeRateService _service;

    public CreateExchangeRateCommandHandler(IExchangeRateService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateExchangeRateCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
