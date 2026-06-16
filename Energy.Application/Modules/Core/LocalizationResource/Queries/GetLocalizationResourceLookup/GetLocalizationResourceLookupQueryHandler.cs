using Energy.Application.Modules.Core.LocalizationResource.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.LocalizationResource.Queries.GetLocalizationResourceLookup;

/// <summary>
/// <see cref="GetLocalizationResourceLookupQuery"/> handler'ı. <see cref="ILocalizationResourceLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetLocalizationResourceLookupQueryHandler
    : IRequestHandler<GetLocalizationResourceLookupQuery, BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>>
{
    private readonly ILocalizationResourceLookupService _lookup;

    public GetLocalizationResourceLookupQueryHandler(ILocalizationResourceLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>> Handle(
        GetLocalizationResourceLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
