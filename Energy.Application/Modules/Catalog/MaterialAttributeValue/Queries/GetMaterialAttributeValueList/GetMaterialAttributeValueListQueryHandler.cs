using Energy.Application.Modules.Catalog.MaterialAttributeValue.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueList;

/// <summary>
/// <see cref="GetMaterialAttributeValueListQuery"/> handler'ı. <see cref="IMaterialAttributeValueService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeValueListQueryHandler
    : IRequestHandler<GetMaterialAttributeValueListQuery, BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>>
{
    private readonly IMaterialAttributeValueService _service;

    public GetMaterialAttributeValueListQueryHandler(IMaterialAttributeValueService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>> Handle(
        GetMaterialAttributeValueListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
