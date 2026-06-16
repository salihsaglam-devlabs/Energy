using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.UpdateProgressPaymentDeduction;

/// <summary>
/// <see cref="UpdateProgressPaymentDeductionCommand"/> handler'ı. <see cref="IProgressPaymentDeductionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProgressPaymentDeductionCommandHandler
    : IRequestHandler<UpdateProgressPaymentDeductionCommand, BaseResponse<bool>>
{
    private readonly IProgressPaymentDeductionService _service;

    public UpdateProgressPaymentDeductionCommandHandler(IProgressPaymentDeductionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProgressPaymentDeductionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
