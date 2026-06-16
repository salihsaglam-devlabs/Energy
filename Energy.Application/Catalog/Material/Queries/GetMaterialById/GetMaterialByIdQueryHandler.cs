using Energy.Application.Catalog.Material.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Queries.GetMaterialById;

/// <summary>
/// <see cref="GetMaterialByIdQuery"/> handler'ı. <see cref="IMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class GetMaterialByIdQueryHandler
    : IRequestHandler<GetMaterialByIdQuery, BaseResponse<MaterialDetailResponse>>
{
    private readonly IMaterialService _service;

    public GetMaterialByIdQueryHandler(IMaterialService service)
        => _service = service;

    public Task<BaseResponse<MaterialDetailResponse>> Handle(
        GetMaterialByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
