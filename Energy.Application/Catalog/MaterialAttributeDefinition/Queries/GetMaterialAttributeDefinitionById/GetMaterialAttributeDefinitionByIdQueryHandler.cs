using Energy.Application.Catalog.MaterialAttributeDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionById;

/// <summary>
/// <see cref="GetMaterialAttributeDefinitionByIdQuery"/> handler'ı. <see cref="IMaterialAttributeDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeDefinitionByIdQueryHandler
    : IRequestHandler<GetMaterialAttributeDefinitionByIdQuery, BaseResponse<MaterialAttributeDefinitionDetailResponse>>
{
    private readonly IMaterialAttributeDefinitionService _service;

    public GetMaterialAttributeDefinitionByIdQueryHandler(IMaterialAttributeDefinitionService service)
        => _service = service;

    public Task<BaseResponse<MaterialAttributeDefinitionDetailResponse>> Handle(
        GetMaterialAttributeDefinitionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
