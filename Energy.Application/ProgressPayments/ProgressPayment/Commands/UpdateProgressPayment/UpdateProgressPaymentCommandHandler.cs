using Energy.Application.ProgressPayments.ProgressPayment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPayment.Commands.UpdateProgressPayment;

/// <summary>
/// <see cref="UpdateProgressPaymentCommand"/> handler'ı. <see cref="IProgressPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProgressPaymentCommandHandler
    : IRequestHandler<UpdateProgressPaymentCommand, BaseResponse<bool>>
{
    private readonly IProgressPaymentService _service;

    public UpdateProgressPaymentCommandHandler(IProgressPaymentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProgressPaymentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
