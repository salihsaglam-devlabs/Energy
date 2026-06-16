using Energy.Application.Modules.Core.Currency.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Currency.Commands.UpdateCurrency;

/// <summary>
/// <see cref="UpdateCurrencyCommand"/> handler'ı. <see cref="ICurrencyService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateCurrencyCommandHandler
    : IRequestHandler<UpdateCurrencyCommand, BaseResponse<bool>>
{
    private readonly ICurrencyService _service;

    public UpdateCurrencyCommandHandler(ICurrencyService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateCurrencyCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
