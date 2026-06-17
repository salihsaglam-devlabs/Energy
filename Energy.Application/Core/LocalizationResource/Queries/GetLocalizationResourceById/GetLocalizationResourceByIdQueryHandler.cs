using Energy.Application.Core.LocalizationResource.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Queries.GetLocalizationResourceById;

/// <summary>
/// <see cref="GetLocalizationResourceByIdQuery"/> handler'ı. <see cref="ILocalizationResourceService"/>'i orkestre eder.
/// </summary>
public sealed class GetLocalizationResourceByIdQueryHandler
    : IRequestHandler<GetLocalizationResourceByIdQuery, BaseResponse<LocalizationResourceDetailResponse>>
{
    private readonly ILocalizationResourceService _service;

    public GetLocalizationResourceByIdQueryHandler(ILocalizationResourceService service)
        => _service = service;

    public Task<BaseResponse<LocalizationResourceDetailResponse>> Handle(
        GetLocalizationResourceByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
