using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Commands.UpdateProgressPaymentLine;

/// <summary>
/// <see cref="UpdateProgressPaymentLineCommand"/> handler'ı. <see cref="IProgressPaymentLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProgressPaymentLineCommandHandler
    : IRequestHandler<UpdateProgressPaymentLineCommand, BaseResponse<bool>>
{
    private readonly IProgressPaymentLineService _service;

    public UpdateProgressPaymentLineCommandHandler(IProgressPaymentLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProgressPaymentLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
