using Energy.Application.Catalog.MaterialAttributeOption.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Commands.CreateMaterialAttributeOption;

/// <summary>
/// <see cref="CreateMaterialAttributeOptionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMaterialAttributeOptionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMaterialAttributeOptionCommandHandler
    : IRequestHandler<CreateMaterialAttributeOptionCommand, BaseResponse<Guid>>
{
    private readonly IMaterialAttributeOptionService _service;

    public CreateMaterialAttributeOptionCommandHandler(IMaterialAttributeOptionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMaterialAttributeOptionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
