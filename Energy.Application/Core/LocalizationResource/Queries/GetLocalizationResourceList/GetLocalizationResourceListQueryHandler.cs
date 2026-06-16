using Energy.Application.Core.LocalizationResource.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Queries.GetLocalizationResourceList;

/// <summary>
/// <see cref="GetLocalizationResourceListQuery"/> handler'ı. <see cref="ILocalizationResourceService"/>'i orkestre eder.
/// </summary>
public sealed class GetLocalizationResourceListQueryHandler
    : IRequestHandler<GetLocalizationResourceListQuery, BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>>
{
    private readonly ILocalizationResourceService _service;

    public GetLocalizationResourceListQueryHandler(ILocalizationResourceService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>> Handle(
        GetLocalizationResourceListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
