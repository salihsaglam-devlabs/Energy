using Energy.Application.Modules.Contracts.ContractLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractLine.Queries.GetContractLineLookup;

/// <summary>
/// <see cref="GetContractLineLookupQuery"/> handler'ı. <see cref="IContractLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractLineLookupQueryHandler
    : IRequestHandler<GetContractLineLookupQuery, BaseResponse<IReadOnlyList<ContractLineLookupResponse>>>
{
    private readonly IContractLineLookupService _lookup;

    public GetContractLineLookupQueryHandler(IContractLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ContractLineLookupResponse>>> Handle(
        GetContractLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
