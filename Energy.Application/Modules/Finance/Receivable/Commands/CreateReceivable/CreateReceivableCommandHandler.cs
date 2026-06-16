using Energy.Application.Modules.Finance.Receivable.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Receivable.Commands.CreateReceivable;

/// <summary>
/// <see cref="CreateReceivableCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IReceivableService"/>'i orkestre eder.
/// </summary>
public sealed class CreateReceivableCommandHandler
    : IRequestHandler<CreateReceivableCommand, BaseResponse<Guid>>
{
    private readonly IReceivableService _service;

    public CreateReceivableCommandHandler(IReceivableService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateReceivableCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
