using Energy.Application.Core.UnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.UnitConversion.Commands.CreateUnitConversion;

/// <summary>
/// <see cref="CreateUnitConversionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateUnitConversionCommandHandler
    : IRequestHandler<CreateUnitConversionCommand, BaseResponse<Guid>>
{
    private readonly IUnitConversionService _service;

    public CreateUnitConversionCommandHandler(IUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateUnitConversionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
