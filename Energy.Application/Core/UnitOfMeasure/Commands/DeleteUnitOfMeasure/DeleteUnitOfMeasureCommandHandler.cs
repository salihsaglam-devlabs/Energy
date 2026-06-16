using Energy.Application.Core.UnitOfMeasure.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.UnitOfMeasure.Commands.DeleteUnitOfMeasure;

/// <summary>
/// <see cref="DeleteUnitOfMeasureCommand"/> handler'ı. <see cref="IUnitOfMeasureService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteUnitOfMeasureCommandHandler
    : IRequestHandler<DeleteUnitOfMeasureCommand, BaseResponse<bool>>
{
    private readonly IUnitOfMeasureService _service;

    public DeleteUnitOfMeasureCommandHandler(IUnitOfMeasureService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteUnitOfMeasureCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
