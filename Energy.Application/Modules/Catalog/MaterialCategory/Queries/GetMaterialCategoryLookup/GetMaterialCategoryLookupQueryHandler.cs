using Energy.Application.Modules.Catalog.MaterialCategory.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Queries.GetMaterialCategoryLookup;

/// <summary>
/// <see cref="GetMaterialCategoryLookupQuery"/> handler'ı. <see cref="IMaterialCategoryLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialCategoryLookupQueryHandler
    : IRequestHandler<GetMaterialCategoryLookupQuery, BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>>
{
    private readonly IMaterialCategoryLookupService _lookup;

    public GetMaterialCategoryLookupQueryHandler(IMaterialCategoryLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>> Handle(
        GetMaterialCategoryLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
