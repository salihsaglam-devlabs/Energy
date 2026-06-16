using Energy.Application.Modules.Core.UnitOfMeasure.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.UnitOfMeasure.Commands.CreateUnitOfMeasure;

/// <summary>
/// <see cref="CreateUnitOfMeasureCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IUnitOfMeasureService"/>'i orkestre eder.
/// </summary>
public sealed class CreateUnitOfMeasureCommandHandler
    : IRequestHandler<CreateUnitOfMeasureCommand, BaseResponse<Guid>>
{
    private readonly IUnitOfMeasureService _service;

    public CreateUnitOfMeasureCommandHandler(IUnitOfMeasureService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateUnitOfMeasureCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
