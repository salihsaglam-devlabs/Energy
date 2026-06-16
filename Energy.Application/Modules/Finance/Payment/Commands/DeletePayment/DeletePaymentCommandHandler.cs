using Energy.Application.Modules.Finance.Payment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Commands.DeletePayment;

/// <summary>
/// <see cref="DeletePaymentCommand"/> handler'ı. <see cref="IPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePaymentCommandHandler
    : IRequestHandler<DeletePaymentCommand, BaseResponse<bool>>
{
    private readonly IPaymentService _service;

    public DeletePaymentCommandHandler(IPaymentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePaymentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
