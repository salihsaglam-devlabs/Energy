using Energy.Application.Catalog.Material.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Commands.CreateMaterial;

/// <summary>
/// <see cref="CreateMaterialCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialCommandHandler
    : IRequestHandler<CreateMaterialCommand, BaseResponse<Guid>>
{
    private readonly IMaterialService _service;

    public CreateMaterialCommandHandler(IMaterialService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
