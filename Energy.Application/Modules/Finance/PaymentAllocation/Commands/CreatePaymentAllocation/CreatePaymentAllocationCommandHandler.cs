using Energy.Application.Modules.Finance.PaymentAllocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Commands.CreatePaymentAllocation;

/// <summary>
/// <see cref="CreatePaymentAllocationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPaymentAllocationService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePaymentAllocationCommandHandler
    : IRequestHandler<CreatePaymentAllocationCommand, BaseResponse<Guid>>
{
    private readonly IPaymentAllocationService _service;

    public CreatePaymentAllocationCommandHandler(IPaymentAllocationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePaymentAllocationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
