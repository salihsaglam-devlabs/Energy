using Energy.Application.Modules.ProgressPayments.ProgressPayment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Commands.DeleteProgressPayment;

/// <summary>
/// <see cref="DeleteProgressPaymentCommand"/> handler'ı. <see cref="IProgressPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProgressPaymentCommandHandler
    : IRequestHandler<DeleteProgressPaymentCommand, BaseResponse<bool>>
{
    private readonly IProgressPaymentService _service;

    public DeleteProgressPaymentCommandHandler(IProgressPaymentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProgressPaymentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
