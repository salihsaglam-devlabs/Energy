using Energy.Application.Core.Company.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Responses;
using MediatR;

namespace Energy.Application.Core.Company.Queries.GetCompanyLookup;

/// <summary>
/// <see cref="GetCompanyLookupQuery"/> handler'ı. <see cref="ICompanyLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetCompanyLookupQueryHandler
    : IRequestHandler<GetCompanyLookupQuery, BaseResponse<IReadOnlyList<CompanyLookupResponse>>>
{
    private readonly ICompanyLookupService _lookup;

    public GetCompanyLookupQueryHandler(ICompanyLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<CompanyLookupResponse>>> Handle(
        GetCompanyLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
