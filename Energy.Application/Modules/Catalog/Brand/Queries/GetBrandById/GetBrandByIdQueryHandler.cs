using Energy.Application.Modules.Catalog.Brand.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.Brand.Queries.GetBrandById;

/// <summary>
/// <see cref="GetBrandByIdQuery"/> handler'ı. <see cref="IBrandService"/>'i orkestre eder.
/// </summary>
public sealed class GetBrandByIdQueryHandler
    : IRequestHandler<GetBrandByIdQuery, BaseResponse<BrandDetailResponse>>
{
    private readonly IBrandService _service;

    public GetBrandByIdQueryHandler(IBrandService service)
        => _service = service;

    public Task<BaseResponse<BrandDetailResponse>> Handle(
        GetBrandByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
