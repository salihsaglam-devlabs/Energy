using Energy.Application.Modules.Finance.Payable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Commands.DeletePayable;

/// <summary>
/// <see cref="DeletePayableCommand"/> handler'ı. <see cref="IPayableService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePayableCommandHandler
    : IRequestHandler<DeletePayableCommand, BaseResponse<bool>>
{
    private readonly IPayableService _service;

    public DeletePayableCommandHandler(IPayableService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePayableCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
