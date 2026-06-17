using Energy.Application.Catalog.MaterialAttributeDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Commands.UpdateMaterialAttributeDefinition;

/// <summary>
/// <see cref="UpdateMaterialAttributeDefinitionCommand"/> handler'ı. <see cref="IMaterialAttributeDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialAttributeDefinitionCommandHandler
    : IRequestHandler<UpdateMaterialAttributeDefinitionCommand, BaseResponse<bool>>
{
    private readonly IMaterialAttributeDefinitionService _service;

    public UpdateMaterialAttributeDefinitionCommandHandler(IMaterialAttributeDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialAttributeDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
