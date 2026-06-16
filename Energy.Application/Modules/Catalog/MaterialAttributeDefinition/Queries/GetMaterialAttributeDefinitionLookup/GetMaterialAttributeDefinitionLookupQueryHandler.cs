using Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionLookup;

/// <summary>
/// <see cref="GetMaterialAttributeDefinitionLookupQuery"/> handler'ı. <see cref="IMaterialAttributeDefinitionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeDefinitionLookupQueryHandler
    : IRequestHandler<GetMaterialAttributeDefinitionLookupQuery, BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>>
{
    private readonly IMaterialAttributeDefinitionLookupService _lookup;

    public GetMaterialAttributeDefinitionLookupQueryHandler(IMaterialAttributeDefinitionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialAttributeDefinitionLookupResponse>>> Handle(
        GetMaterialAttributeDefinitionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
