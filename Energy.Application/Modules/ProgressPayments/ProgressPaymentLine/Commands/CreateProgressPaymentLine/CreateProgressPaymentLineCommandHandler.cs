using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.CreateProgressPaymentLine;

/// <summary>
/// <see cref="CreateProgressPaymentLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProgressPaymentLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProgressPaymentLineCommandHandler
    : IRequestHandler<CreateProgressPaymentLineCommand, BaseResponse<Guid>>
{
    private readonly IProgressPaymentLineService _service;

    public CreateProgressPaymentLineCommandHandler(IProgressPaymentLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProgressPaymentLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
