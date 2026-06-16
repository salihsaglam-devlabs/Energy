using Energy.Application.Modules.Catalog.MaterialAttributeOption.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Commands.UpdateMaterialAttributeOption;

/// <summary>
/// <see cref="UpdateMaterialAttributeOptionCommand"/> handler'ı. <see cref="IMaterialAttributeOptionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialAttributeOptionCommandHandler
    : IRequestHandler<UpdateMaterialAttributeOptionCommand, BaseResponse<bool>>
{
    private readonly IMaterialAttributeOptionService _service;

    public UpdateMaterialAttributeOptionCommandHandler(IMaterialAttributeOptionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialAttributeOptionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
