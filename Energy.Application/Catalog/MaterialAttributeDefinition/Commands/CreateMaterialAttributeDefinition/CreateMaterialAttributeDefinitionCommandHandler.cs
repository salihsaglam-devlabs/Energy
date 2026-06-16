using Energy.Application.Catalog.MaterialAttributeDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Commands.CreateMaterialAttributeDefinition;

/// <summary>
/// <see cref="CreateMaterialAttributeDefinitionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialAttributeDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialAttributeDefinitionCommandHandler
    : IRequestHandler<CreateMaterialAttributeDefinitionCommand, BaseResponse<Guid>>
{
    private readonly IMaterialAttributeDefinitionService _service;

    public CreateMaterialAttributeDefinitionCommandHandler(IMaterialAttributeDefinitionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialAttributeDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
