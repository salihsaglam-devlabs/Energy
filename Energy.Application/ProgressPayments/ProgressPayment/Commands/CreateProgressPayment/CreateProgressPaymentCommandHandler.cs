using Energy.Application.ProgressPayments.ProgressPayment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPayment.Commands.CreateProgressPayment;

/// <summary>
/// <see cref="CreateProgressPaymentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProgressPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProgressPaymentCommandHandler
    : IRequestHandler<CreateProgressPaymentCommand, BaseResponse<Guid>>
{
    private readonly IProgressPaymentService _service;

    public CreateProgressPaymentCommandHandler(IProgressPaymentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProgressPaymentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
