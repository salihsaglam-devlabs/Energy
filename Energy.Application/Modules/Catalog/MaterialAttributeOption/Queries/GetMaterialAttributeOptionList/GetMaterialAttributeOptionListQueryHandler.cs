using Energy.Application.Modules.Catalog.MaterialAttributeOption.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Queries.GetMaterialAttributeOptionList;

/// <summary>
/// <see cref="GetMaterialAttributeOptionListQuery"/> handler'ı. <see cref="IMaterialAttributeOptionService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialAttributeOptionListQueryHandler
    : IRequestHandler<GetMaterialAttributeOptionListQuery, BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>>
{
    private readonly IMaterialAttributeOptionService _service;

    public GetMaterialAttributeOptionListQueryHandler(IMaterialAttributeOptionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>> Handle(
        GetMaterialAttributeOptionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
