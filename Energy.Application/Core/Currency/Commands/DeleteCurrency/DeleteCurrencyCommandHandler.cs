using Energy.Application.Core.Currency.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Currency.Commands.DeleteCurrency;

/// <summary>
/// <see cref="DeleteCurrencyCommand"/> handler'ı. <see cref="ICurrencyService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteCurrencyCommandHandler
    : IRequestHandler<DeleteCurrencyCommand, BaseResponse<bool>>
{
    private readonly ICurrencyService _service;

    public DeleteCurrencyCommandHandler(ICurrencyService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteCurrencyCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
