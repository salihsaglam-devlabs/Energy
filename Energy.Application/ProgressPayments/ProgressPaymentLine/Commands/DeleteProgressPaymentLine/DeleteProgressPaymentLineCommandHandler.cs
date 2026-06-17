using Energy.Application.ProgressPayments.ProgressPaymentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentLine.Commands.DeleteProgressPaymentLine;

/// <summary>
/// <see cref="DeleteProgressPaymentLineCommand"/> handler'ı. <see cref="IProgressPaymentLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProgressPaymentLineCommandHandler
    : IRequestHandler<DeleteProgressPaymentLineCommand, BaseResponse<bool>>
{
    private readonly IProgressPaymentLineService _service;

    public DeleteProgressPaymentLineCommandHandler(IProgressPaymentLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProgressPaymentLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
