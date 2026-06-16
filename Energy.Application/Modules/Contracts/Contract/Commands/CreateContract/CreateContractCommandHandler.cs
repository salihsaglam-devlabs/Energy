using Energy.Application.Modules.Contracts.Contract.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Commands.CreateContract;

/// <summary>
/// <see cref="CreateContractCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IContractService"/>'i orkestre eder.
/// </summary>
public sealed class CreateContractCommandHandler
    : IRequestHandler<CreateContractCommand, BaseResponse<Guid>>
{
    private readonly IContractService _service;

    public CreateContractCommandHandler(IContractService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateContractCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
