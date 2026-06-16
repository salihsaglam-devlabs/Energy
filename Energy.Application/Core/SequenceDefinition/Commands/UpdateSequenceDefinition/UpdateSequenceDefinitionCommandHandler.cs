using Energy.Application.Core.SequenceDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Commands.UpdateSequenceDefinition;

/// <summary>
/// <see cref="UpdateSequenceDefinitionCommand"/> handler'ı. <see cref="ISequenceDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateSequenceDefinitionCommandHandler
    : IRequestHandler<UpdateSequenceDefinitionCommand, BaseResponse<bool>>
{
    private readonly ISequenceDefinitionService _service;

    public UpdateSequenceDefinitionCommandHandler(ISequenceDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateSequenceDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
