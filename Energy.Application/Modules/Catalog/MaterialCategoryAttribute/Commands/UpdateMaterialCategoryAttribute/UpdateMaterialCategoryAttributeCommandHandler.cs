using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.UpdateMaterialCategoryAttribute;

/// <summary>
/// <see cref="UpdateMaterialCategoryAttributeCommand"/> handler'ı. <see cref="IMaterialCategoryAttributeService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialCategoryAttributeCommandHandler
    : IRequestHandler<UpdateMaterialCategoryAttributeCommand, BaseResponse<bool>>
{
    private readonly IMaterialCategoryAttributeService _service;

    public UpdateMaterialCategoryAttributeCommandHandler(IMaterialCategoryAttributeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialCategoryAttributeCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
