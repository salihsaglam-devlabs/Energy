using Energy.Application.Catalog.Material.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.Material.Commands.DeleteMaterial;

/// <summary>
/// <see cref="DeleteMaterialCommand"/> handler'ı. <see cref="IMaterialService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialCommandHandler
    : IRequestHandler<DeleteMaterialCommand, BaseResponse<bool>>
{
    private readonly IMaterialService _service;

    public DeleteMaterialCommandHandler(IMaterialService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
