using Energy.Application.Core.ExchangeRate.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Commands.UpdateExchangeRate;

/// <summary>
/// <see cref="UpdateExchangeRateCommand"/> handler'ı. <see cref="IExchangeRateService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateExchangeRateCommandHandler
    : IRequestHandler<UpdateExchangeRateCommand, BaseResponse<bool>>
{
    private readonly IExchangeRateService _service;

    public UpdateExchangeRateCommandHandler(IExchangeRateService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateExchangeRateCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
