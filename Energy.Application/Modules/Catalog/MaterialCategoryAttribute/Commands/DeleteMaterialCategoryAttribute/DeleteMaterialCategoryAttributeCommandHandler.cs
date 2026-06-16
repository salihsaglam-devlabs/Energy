using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.DeleteMaterialCategoryAttribute;

/// <summary>
/// <see cref="DeleteMaterialCategoryAttributeCommand"/> handler'ı. <see cref="IMaterialCategoryAttributeService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialCategoryAttributeCommandHandler
    : IRequestHandler<DeleteMaterialCategoryAttributeCommand, BaseResponse<bool>>
{
    private readonly IMaterialCategoryAttributeService _service;

    public DeleteMaterialCategoryAttributeCommandHandler(IMaterialCategoryAttributeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialCategoryAttributeCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
