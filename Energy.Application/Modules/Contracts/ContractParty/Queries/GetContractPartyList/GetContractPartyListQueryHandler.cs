using Energy.Application.Modules.Contracts.ContractParty.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyList;

/// <summary>
/// <see cref="GetContractPartyListQuery"/> handler'ı. <see cref="IContractPartyService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractPartyListQueryHandler
    : IRequestHandler<GetContractPartyListQuery, BaseResponse<PaginatedResponse<ContractPartyListResponse>>>
{
    private readonly IContractPartyService _service;

    public GetContractPartyListQueryHandler(IContractPartyService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ContractPartyListResponse>>> Handle(
        GetContractPartyListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
