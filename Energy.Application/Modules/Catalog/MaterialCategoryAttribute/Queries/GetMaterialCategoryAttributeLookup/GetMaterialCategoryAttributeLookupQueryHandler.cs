using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeLookup;

/// <summary>
/// <see cref="GetMaterialCategoryAttributeLookupQuery"/> handler'ı. <see cref="IMaterialCategoryAttributeLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialCategoryAttributeLookupQueryHandler
    : IRequestHandler<GetMaterialCategoryAttributeLookupQuery, BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>>
{
    private readonly IMaterialCategoryAttributeLookupService _lookup;

    public GetMaterialCategoryAttributeLookupQueryHandler(IMaterialCategoryAttributeLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>> Handle(
        GetMaterialCategoryAttributeLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
