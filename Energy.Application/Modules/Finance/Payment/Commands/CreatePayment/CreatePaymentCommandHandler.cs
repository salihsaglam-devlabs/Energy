using Energy.Application.Modules.Finance.Payment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Commands.CreatePayment;

/// <summary>
/// <see cref="CreatePaymentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPaymentService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, BaseResponse<Guid>>
{
    private readonly IPaymentService _service;

    public CreatePaymentCommandHandler(IPaymentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
