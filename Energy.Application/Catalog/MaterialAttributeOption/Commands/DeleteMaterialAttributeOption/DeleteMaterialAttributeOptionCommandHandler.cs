using Energy.Application.Catalog.MaterialAttributeOption.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Commands.DeleteMaterialAttributeOption;

/// <summary>
/// <see cref="DeleteMaterialAttributeOptionCommand"/> handler'ı. <see cref="IMaterialAttributeOptionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialAttributeOptionCommandHandler
    : IRequestHandler<DeleteMaterialAttributeOptionCommand, BaseResponse<bool>>
{
    private readonly IMaterialAttributeOptionService _service;

    public DeleteMaterialAttributeOptionCommandHandler(IMaterialAttributeOptionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialAttributeOptionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
