using Energy.Application.Core.ExchangeRate.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.ExchangeRate.Commands.DeleteExchangeRate;

/// <summary>
/// <see cref="DeleteExchangeRateCommand"/> handler'ı. <see cref="IExchangeRateService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteExchangeRateCommandHandler
    : IRequestHandler<DeleteExchangeRateCommand, BaseResponse<bool>>
{
    private readonly IExchangeRateService _service;

    public DeleteExchangeRateCommandHandler(IExchangeRateService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteExchangeRateCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
