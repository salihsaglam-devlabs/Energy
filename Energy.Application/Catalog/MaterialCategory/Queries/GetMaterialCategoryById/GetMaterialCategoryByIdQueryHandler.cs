using Energy.Application.Catalog.MaterialCategory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialCategory.Queries.GetMaterialCategoryById;

/// <summary>
/// <see cref="GetMaterialCategoryByIdQuery"/> handler'ı. <see cref="IMaterialCategoryService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialCategoryByIdQueryHandler
    : IRequestHandler<GetMaterialCategoryByIdQuery, BaseResponse<MaterialCategoryDetailResponse>>
{
    private readonly IMaterialCategoryService _service;

    public GetMaterialCategoryByIdQueryHandler(IMaterialCategoryService service)
        => _service = service;

    public Task<BaseResponse<MaterialCategoryDetailResponse>> Handle(
        GetMaterialCategoryByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
