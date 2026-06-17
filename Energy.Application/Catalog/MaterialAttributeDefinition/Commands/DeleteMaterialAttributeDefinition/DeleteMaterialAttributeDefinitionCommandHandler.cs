using Energy.Application.Catalog.MaterialAttributeDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Commands.DeleteMaterialAttributeDefinition;

/// <summary>
/// <see cref="DeleteMaterialAttributeDefinitionCommand"/> handler'ı. <see cref="IMaterialAttributeDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialAttributeDefinitionCommandHandler
    : IRequestHandler<DeleteMaterialAttributeDefinitionCommand, BaseResponse<bool>>
{
    private readonly IMaterialAttributeDefinitionService _service;

    public DeleteMaterialAttributeDefinitionCommandHandler(IMaterialAttributeDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialAttributeDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
