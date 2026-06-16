using Energy.Application.Modules.Contracts.ContractAmendment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Responses;
using MediatR;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Queries.GetContractAmendmentLookup;

/// <summary>
/// <see cref="GetContractAmendmentLookupQuery"/> handler'ı. <see cref="IContractAmendmentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetContractAmendmentLookupQueryHandler
    : IRequestHandler<GetContractAmendmentLookupQuery, BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>>
{
    private readonly IContractAmendmentLookupService _lookup;

    public GetContractAmendmentLookupQueryHandler(IContractAmendmentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ContractAmendmentLookupResponse>>> Handle(
        GetContractAmendmentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
