using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.CreateProgressPaymentDeduction;

/// <summary>
/// <see cref="CreateProgressPaymentDeductionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProgressPaymentDeductionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProgressPaymentDeductionCommandHandler
    : IRequestHandler<CreateProgressPaymentDeductionCommand, BaseResponse<Guid>>
{
    private readonly IProgressPaymentDeductionService _service;

    public CreateProgressPaymentDeductionCommandHandler(IProgressPaymentDeductionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProgressPaymentDeductionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
