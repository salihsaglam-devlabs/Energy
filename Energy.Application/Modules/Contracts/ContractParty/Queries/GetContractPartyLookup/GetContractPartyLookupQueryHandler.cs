using Energy.Application.Modules.Contracts.ContractParty.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractParty.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractParty.Queries.GetContractPartyLookup;

/// <summary>
/// <see cref="GetContractPartyLookupQuery"/> handler'ı. <see cref="IContractPartyLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractPartyLookupQueryHandler
    : IRequestHandler<GetContractPartyLookupQuery, BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>>
{
    private readonly IContractPartyLookupService _lookup;

    public GetContractPartyLookupQueryHandler(IContractPartyLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ContractPartyLookupResponse>>> Handle(
        GetContractPartyLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
