using Energy.Application.Modules.Catalog.MaterialAttributeValue.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeValue.Commands.UpdateMaterialAttributeValue;

/// <summary>
/// <see cref="UpdateMaterialAttributeValueCommand"/> handler'ı. <see cref="IMaterialAttributeValueService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMaterialAttributeValueCommandHandler
    : IRequestHandler<UpdateMaterialAttributeValueCommand, BaseResponse<bool>>
{
    private readonly IMaterialAttributeValueService _service;

    public UpdateMaterialAttributeValueCommandHandler(IMaterialAttributeValueService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMaterialAttributeValueCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
