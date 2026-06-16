using Energy.Application.Modules.Core.UnitConversion.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitConversion.Commands.DeleteUnitConversion;

/// <summary>
/// <see cref="DeleteUnitConversionCommand"/> handler'ı. <see cref="IUnitConversionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteUnitConversionCommandHandler
    : IRequestHandler<DeleteUnitConversionCommand, BaseResponse<bool>>
{
    private readonly IUnitConversionService _service;

    public DeleteUnitConversionCommandHandler(IUnitConversionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteUnitConversionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
