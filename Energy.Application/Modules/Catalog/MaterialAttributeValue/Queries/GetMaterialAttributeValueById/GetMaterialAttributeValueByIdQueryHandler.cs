using Energy.Application.Modules.Catalog.MaterialAttributeValue.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueById;

/// <summary>
/// <see cref="GetMaterialAttributeValueByIdQuery"/> handler'ı. <see cref="IMaterialAttributeValueService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeValueByIdQueryHandler
    : IRequestHandler<GetMaterialAttributeValueByIdQuery, BaseResponse<MaterialAttributeValueDetailResponse>>
{
    private readonly IMaterialAttributeValueService _service;

    public GetMaterialAttributeValueByIdQueryHandler(IMaterialAttributeValueService service)
        => _service = service;

    public Task<BaseResponse<MaterialAttributeValueDetailResponse>> Handle(
        GetMaterialAttributeValueByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
