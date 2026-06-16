using Energy.Application.Modules.Catalog.Brand.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.Brand.Queries.GetBrandList;

/// <summary>
/// <see cref="GetBrandListQuery"/> handler'ı. <see cref="IBrandService"/>'i orkestre eder.
/// </summary>
public sealed class GetBrandListQueryHandler
    : IRequestHandler<GetBrandListQuery, BaseResponse<PaginatedResponse<BrandListResponse>>>
{
    private readonly IBrandService _service;

    public GetBrandListQueryHandler(IBrandService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BrandListResponse>>> Handle(
        GetBrandListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
