using Energy.Application.Finance.Payable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.Payable.Commands.UpdatePayable;

/// <summary>
/// <see cref="UpdatePayableCommand"/> handler'ı. <see cref="IPayableService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePayableCommandHandler
    : IRequestHandler<UpdatePayableCommand, BaseResponse<bool>>
{
    private readonly IPayableService _service;

    public UpdatePayableCommandHandler(IPayableService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePayableCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
