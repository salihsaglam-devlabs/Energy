using Energy.Application.Core.LocalizationResource.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Commands.DeleteLocalizationResource;

/// <summary>
/// <see cref="DeleteLocalizationResourceCommand"/> handler'ı. <see cref="ILocalizationResourceService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteLocalizationResourceCommandHandler
    : IRequestHandler<DeleteLocalizationResourceCommand, BaseResponse<bool>>
{
    private readonly ILocalizationResourceService _service;

    public DeleteLocalizationResourceCommandHandler(ILocalizationResourceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteLocalizationResourceCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
