using Energy.Application.Catalog.Material.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Queries.GetMaterialList;

/// <summary>
/// <see cref="GetMaterialListQuery"/> handler'ı. <see cref="IMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialListQueryHandler
    : IRequestHandler<GetMaterialListQuery, BaseResponse<PaginatedResponse<MaterialListResponse>>>
{
    private readonly IMaterialService _service;

    public GetMaterialListQueryHandler(IMaterialService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<MaterialListResponse>>> Handle(
        GetMaterialListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
