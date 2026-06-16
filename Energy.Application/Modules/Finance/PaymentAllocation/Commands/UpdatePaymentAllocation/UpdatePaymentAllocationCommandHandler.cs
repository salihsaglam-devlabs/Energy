using Energy.Application.Modules.Finance.PaymentAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Commands.UpdatePaymentAllocation;

/// <summary>
/// <see cref="UpdatePaymentAllocationCommand"/> handler'ı. <see cref="IPaymentAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePaymentAllocationCommandHandler
    : IRequestHandler<UpdatePaymentAllocationCommand, BaseResponse<bool>>
{
    private readonly IPaymentAllocationService _service;

    public UpdatePaymentAllocationCommandHandler(IPaymentAllocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePaymentAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
