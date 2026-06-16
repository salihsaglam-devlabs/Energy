using Energy.Application.Catalog.MaterialCategory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialCategory.Commands.UpdateMaterialCategory;

/// <summary>
/// <see cref="UpdateMaterialCategoryCommand"/> handler'ı. <see cref="IMaterialCategoryService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialCategoryCommandHandler
    : IRequestHandler<UpdateMaterialCategoryCommand, BaseResponse<bool>>
{
    private readonly IMaterialCategoryService _service;

    public UpdateMaterialCategoryCommandHandler(IMaterialCategoryService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialCategoryCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
