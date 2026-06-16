using Energy.Application.Modules.Finance.Payable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payable.Commands.CreatePayable;

/// <summary>
/// <see cref="CreatePayableCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPayableService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePayableCommandHandler
    : IRequestHandler<CreatePayableCommand, BaseResponse<Guid>>
{
    private readonly IPayableService _service;

    public CreatePayableCommandHandler(IPayableService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePayableCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
