using Energy.Application.Modules.Core.UnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Commands.UpdateUnitConversion;

/// <summary>
/// <see cref="UpdateUnitConversionCommand"/> handler'ı. <see cref="IUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateUnitConversionCommandHandler
    : IRequestHandler<UpdateUnitConversionCommand, BaseResponse<bool>>
{
    private readonly IUnitConversionService _service;

    public UpdateUnitConversionCommandHandler(IUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateUnitConversionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
