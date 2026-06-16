using Energy.Application.Core.LocalizationResource.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.LocalizationResource.Commands.CreateLocalizationResource;

/// <summary>
/// <see cref="CreateLocalizationResourceCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ILocalizationResourceService"/>'i orkestre eder.
/// </summary>
public sealed class CreateLocalizationResourceCommandHandler
    : IRequestHandler<CreateLocalizationResourceCommand, BaseResponse<Guid>>
{
    private readonly ILocalizationResourceService _service;

    public CreateLocalizationResourceCommandHandler(ILocalizationResourceService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateLocalizationResourceCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
