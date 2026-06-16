using Energy.Application.Catalog.Material.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Commands.UpdateMaterial;

/// <summary>
/// <see cref="UpdateMaterialCommand"/> handler'ı. <see cref="IMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialCommandHandler
    : IRequestHandler<UpdateMaterialCommand, BaseResponse<bool>>
{
    private readonly IMaterialService _service;

    public UpdateMaterialCommandHandler(IMaterialService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
