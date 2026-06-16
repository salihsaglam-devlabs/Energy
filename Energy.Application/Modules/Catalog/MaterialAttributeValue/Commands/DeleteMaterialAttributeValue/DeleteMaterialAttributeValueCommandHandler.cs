using Energy.Application.Modules.Catalog.MaterialAttributeValue.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialAttributeValue.Commands.DeleteMaterialAttributeValue;

/// <summary>
/// <see cref="DeleteMaterialAttributeValueCommand"/> handler'ı. <see cref="IMaterialAttributeValueService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMaterialAttributeValueCommandHandler
    : IRequestHandler<DeleteMaterialAttributeValueCommand, BaseResponse<bool>>
{
    private readonly IMaterialAttributeValueService _service;

    public DeleteMaterialAttributeValueCommandHandler(IMaterialAttributeValueService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMaterialAttributeValueCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
