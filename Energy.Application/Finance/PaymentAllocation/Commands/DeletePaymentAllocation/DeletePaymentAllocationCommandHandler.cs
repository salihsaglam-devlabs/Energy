using Energy.Application.Finance.PaymentAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.PaymentAllocation.Commands.DeletePaymentAllocation;

/// <summary>
/// <see cref="DeletePaymentAllocationCommand"/> handler'ı. <see cref="IPaymentAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePaymentAllocationCommandHandler
    : IRequestHandler<DeletePaymentAllocationCommand, BaseResponse<bool>>
{
    private readonly IPaymentAllocationService _service;

    public DeletePaymentAllocationCommandHandler(IPaymentAllocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePaymentAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
