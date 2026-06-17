using Energy.Application.Finance.Payment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Payment.Commands.UpdatePayment;

/// <summary>
/// <see cref="UpdatePaymentCommand"/> handler'ı. <see cref="IPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePaymentCommandHandler
    : IRequestHandler<UpdatePaymentCommand, BaseResponse<bool>>
{
    private readonly IPaymentService _service;

    public UpdatePaymentCommandHandler(IPaymentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePaymentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
