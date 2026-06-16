using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeById;

/// <summary>
/// <see cref="GetMaterialCategoryAttributeByIdQuery"/> handler'ı. <see cref="IMaterialCategoryAttributeService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialCategoryAttributeByIdQueryHandler
    : IRequestHandler<GetMaterialCategoryAttributeByIdQuery, BaseResponse<MaterialCategoryAttributeDetailResponse>>
{
    private readonly IMaterialCategoryAttributeService _service;

    public GetMaterialCategoryAttributeByIdQueryHandler(IMaterialCategoryAttributeService service)
        => _service = service;

    public Task<BaseResponse<MaterialCategoryAttributeDetailResponse>> Handle(
        GetMaterialCategoryAttributeByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
