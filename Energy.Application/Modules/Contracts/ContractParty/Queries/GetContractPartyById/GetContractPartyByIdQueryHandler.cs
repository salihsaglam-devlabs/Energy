using Energy.Application.Modules.Contracts.ContractParty.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyById;

/// <summary>
/// <see cref="GetContractPartyByIdQuery"/> handler'ı. <see cref="IContractPartyService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractPartyByIdQueryHandler
    : IRequestHandler<GetContractPartyByIdQuery, BaseResponse<ContractPartyDetailResponse>>
{
    private readonly IContractPartyService _service;

    public GetContractPartyByIdQueryHandler(IContractPartyService service)
        => _service = service;

    public Task<BaseResponse<ContractPartyDetailResponse>> Handle(
        GetContractPartyByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
