using Energy.Application.Modules.Contracts.Contract.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.Contract.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.Contract.Queries.GetContractLookup;

/// <summary>
/// <see cref="GetContractLookupQuery"/> handler'ı. <see cref="IContractLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractLookupQueryHandler
    : IRequestHandler<GetContractLookupQuery, BaseResponse<IReadOnlyList<ContractLookupResponse>>>
{
    private readonly IContractLookupService _lookup;

    public GetContractLookupQueryHandler(IContractLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ContractLookupResponse>>> Handle(
        GetContractLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
