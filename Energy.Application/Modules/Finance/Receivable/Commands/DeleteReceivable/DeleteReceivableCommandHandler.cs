using Energy.Application.Modules.Finance.Receivable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Receivable.Commands.DeleteReceivable;

/// <summary>
/// <see cref="DeleteReceivableCommand"/> handler'ı. <see cref="IReceivableService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteReceivableCommandHandler
    : IRequestHandler<DeleteReceivableCommand, BaseResponse<bool>>
{
    private readonly IReceivableService _service;

    public DeleteReceivableCommandHandler(IReceivableService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteReceivableCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
