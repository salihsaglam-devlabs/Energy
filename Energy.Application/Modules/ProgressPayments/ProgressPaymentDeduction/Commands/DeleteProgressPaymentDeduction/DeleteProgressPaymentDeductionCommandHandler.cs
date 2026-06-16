using Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Commands.DeleteProgressPaymentDeduction;

/// <summary>
/// <see cref="DeleteProgressPaymentDeductionCommand"/> handler'ı. <see cref="IProgressPaymentDeductionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProgressPaymentDeductionCommandHandler
    : IRequestHandler<DeleteProgressPaymentDeductionCommand, BaseResponse<bool>>
{
    private readonly IProgressPaymentDeductionService _service;

    public DeleteProgressPaymentDeductionCommandHandler(IProgressPaymentDeductionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProgressPaymentDeductionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
