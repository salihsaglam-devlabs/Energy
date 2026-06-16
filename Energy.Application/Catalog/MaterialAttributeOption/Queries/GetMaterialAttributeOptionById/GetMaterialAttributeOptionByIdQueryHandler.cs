using Energy.Application.Catalog.MaterialAttributeOption.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Queries.GetMaterialAttributeOptionById;

/// <summary>
/// <see cref="GetMaterialAttributeOptionByIdQuery"/> handler'ı. <see cref="IMaterialAttributeOptionService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeOptionByIdQueryHandler
    : IRequestHandler<GetMaterialAttributeOptionByIdQuery, BaseResponse<MaterialAttributeOptionDetailResponse>>
{
    private readonly IMaterialAttributeOptionService _service;

    public GetMaterialAttributeOptionByIdQueryHandler(IMaterialAttributeOptionService service)
        => _service = service;

    public Task<BaseResponse<MaterialAttributeOptionDetailResponse>> Handle(
        GetMaterialAttributeOptionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
