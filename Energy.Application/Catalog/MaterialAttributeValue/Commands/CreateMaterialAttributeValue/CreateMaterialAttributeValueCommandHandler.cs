using Energy.Application.Catalog.MaterialAttributeValue.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Commands.CreateMaterialAttributeValue;

/// <summary>
/// <see cref="CreateMaterialAttributeValueCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialAttributeValueService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialAttributeValueCommandHandler
    : IRequestHandler<CreateMaterialAttributeValueCommand, BaseResponse<Guid>>
{
    private readonly IMaterialAttributeValueService _service;

    public CreateMaterialAttributeValueCommandHandler(IMaterialAttributeValueService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialAttributeValueCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
