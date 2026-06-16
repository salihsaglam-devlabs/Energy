using Energy.Application.Modules.Contracts.ContractParty.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Commands.CreateContractParty;

/// <summary>
/// <see cref="CreateContractPartyCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IContractPartyService"/>'i orkestre eder.
/// </summary>
public sealed class CreateContractPartyCommandHandler
    : IRequestHandler<CreateContractPartyCommand, BaseResponse<Guid>>
{
    private readonly IContractPartyService _service;

    public CreateContractPartyCommandHandler(IContractPartyService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateContractPartyCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
