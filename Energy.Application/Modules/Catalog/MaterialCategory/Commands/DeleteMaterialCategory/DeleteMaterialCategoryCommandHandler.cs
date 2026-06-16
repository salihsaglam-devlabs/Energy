using Energy.Application.Modules.Catalog.MaterialCategory.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Commands.DeleteMaterialCategory;

/// <summary>
/// <see cref="DeleteMaterialCategoryCommand"/> handler'ı. <see cref="IMaterialCategoryService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialCategoryCommandHandler
    : IRequestHandler<DeleteMaterialCategoryCommand, BaseResponse<bool>>
{
    private readonly IMaterialCategoryService _service;

    public DeleteMaterialCategoryCommandHandler(IMaterialCategoryService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialCategoryCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
