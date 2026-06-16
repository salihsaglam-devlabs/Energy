using Energy.Application.Modules.Catalog.MaterialAttributeOption.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Queries.GetMaterialAttributeOptionLookup;

/// <summary>
/// <see cref="GetMaterialAttributeOptionLookupQuery"/> handler'ı. <see cref="IMaterialAttributeOptionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeOptionLookupQueryHandler
    : IRequestHandler<GetMaterialAttributeOptionLookupQuery, BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>>
{
    private readonly IMaterialAttributeOptionLookupService _lookup;

    public GetMaterialAttributeOptionLookupQueryHandler(IMaterialAttributeOptionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>> Handle(
        GetMaterialAttributeOptionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
