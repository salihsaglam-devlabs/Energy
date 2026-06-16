using Energy.Application.Modules.Catalog.MaterialCategory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Queries.GetMaterialCategoryList;

/// <summary>
/// <see cref="GetMaterialCategoryListQuery"/> handler'ı. <see cref="IMaterialCategoryService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialCategoryListQueryHandler
    : IRequestHandler<GetMaterialCategoryListQuery, BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>>
{
    private readonly IMaterialCategoryService _service;

    public GetMaterialCategoryListQueryHandler(IMaterialCategoryService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>> Handle(
        GetMaterialCategoryListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
