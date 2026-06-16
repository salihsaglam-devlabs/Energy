using Energy.Application.Modules.Core.UnitOfMeasure.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Commands.UpdateUnitOfMeasure;

/// <summary>
/// <see cref="UpdateUnitOfMeasureCommand"/> handler'ı. <see cref="IUnitOfMeasureService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateUnitOfMeasureCommandHandler
    : IRequestHandler<UpdateUnitOfMeasureCommand, BaseResponse<bool>>
{
    private readonly IUnitOfMeasureService _service;

    public UpdateUnitOfMeasureCommandHandler(IUnitOfMeasureService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateUnitOfMeasureCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
