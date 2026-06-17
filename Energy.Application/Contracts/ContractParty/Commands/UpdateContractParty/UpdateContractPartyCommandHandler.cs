using Energy.Application.Contracts.ContractParty.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Contracts.ContractParty.Commands.UpdateContractParty;

/// <summary>
/// <see cref="UpdateContractPartyCommand"/> handler'ı. <see cref="IContractPartyService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateContractPartyCommandHandler
    : IRequestHandler<UpdateContractPartyCommand, BaseResponse<bool>>
{
    private readonly IContractPartyService _service;

    public UpdateContractPartyCommandHandler(IContractPartyService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateContractPartyCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
