using Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Queries.GetMaterialAttributeDefinitionList;

/// <summary>
/// <see cref="GetMaterialAttributeDefinitionListQuery"/> handler'ı. <see cref="IMaterialAttributeDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeDefinitionListQueryHandler
    : IRequestHandler<GetMaterialAttributeDefinitionListQuery, BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>>
{
    private readonly IMaterialAttributeDefinitionService _service;

    public GetMaterialAttributeDefinitionListQueryHandler(IMaterialAttributeDefinitionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialAttributeDefinitionListResponse>>> Handle(
        GetMaterialAttributeDefinitionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
