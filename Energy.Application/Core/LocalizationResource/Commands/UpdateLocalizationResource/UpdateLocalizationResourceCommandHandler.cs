using Energy.Application.Core.LocalizationResource.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Commands.UpdateLocalizationResource;

/// <summary>
/// <see cref="UpdateLocalizationResourceCommand"/> handler'ı. <see cref="ILocalizationResourceService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateLocalizationResourceCommandHandler
    : IRequestHandler<UpdateLocalizationResourceCommand, BaseResponse<bool>>
{
    private readonly ILocalizationResourceService _service;

    public UpdateLocalizationResourceCommandHandler(ILocalizationResourceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateLocalizationResourceCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
