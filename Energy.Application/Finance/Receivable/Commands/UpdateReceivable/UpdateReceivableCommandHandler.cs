using Energy.Application.Finance.Receivable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Receivable.Commands.UpdateReceivable;

/// <summary>
/// <see cref="UpdateReceivableCommand"/> handler'ı. <see cref="IReceivableService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateReceivableCommandHandler
    : IRequestHandler<UpdateReceivableCommand, BaseResponse<bool>>
{
    private readonly IReceivableService _service;

    public UpdateReceivableCommandHandler(IReceivableService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateReceivableCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
