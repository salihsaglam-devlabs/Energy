using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeList;

/// <summary>
/// <see cref="GetMaterialCategoryAttributeListQuery"/> handler'ı. <see cref="IMaterialCategoryAttributeService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialCategoryAttributeListQueryHandler
    : IRequestHandler<GetMaterialCategoryAttributeListQuery, BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>>
{
    private readonly IMaterialCategoryAttributeService _service;

    public GetMaterialCategoryAttributeListQueryHandler(IMaterialCategoryAttributeService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>> Handle(
        GetMaterialCategoryAttributeListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
