using Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Commands.CreateMaterialCategoryAttribute;

/// <summary>
/// <see cref="CreateMaterialCategoryAttributeCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialCategoryAttributeService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialCategoryAttributeCommandHandler
    : IRequestHandler<CreateMaterialCategoryAttributeCommand, BaseResponse<Guid>>
{
    private readonly IMaterialCategoryAttributeService _service;

    public CreateMaterialCategoryAttributeCommandHandler(IMaterialCategoryAttributeService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialCategoryAttributeCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
