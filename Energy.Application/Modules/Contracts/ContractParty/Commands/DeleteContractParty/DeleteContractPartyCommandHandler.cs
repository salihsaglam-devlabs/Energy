using Energy.Application.Modules.Contracts.ContractParty.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Commands.DeleteContractParty;

/// <summary>
/// <see cref="DeleteContractPartyCommand"/> handler'ı. <see cref="IContractPartyService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteContractPartyCommandHandler
    : IRequestHandler<DeleteContractPartyCommand, BaseResponse<bool>>
{
    private readonly IContractPartyService _service;

    public DeleteContractPartyCommandHandler(IContractPartyService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteContractPartyCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
