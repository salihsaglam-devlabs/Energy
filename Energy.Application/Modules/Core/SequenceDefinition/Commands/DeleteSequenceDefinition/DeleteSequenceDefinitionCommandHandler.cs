using Energy.Application.Modules.Core.SequenceDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.SequenceDefinition.Commands.DeleteSequenceDefinition;

/// <summary>
/// <see cref="DeleteSequenceDefinitionCommand"/> handler'ı. <see cref="ISequenceDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteSequenceDefinitionCommandHandler
    : IRequestHandler<DeleteSequenceDefinitionCommand, BaseResponse<bool>>
{
    private readonly ISequenceDefinitionService _service;

    public DeleteSequenceDefinitionCommandHandler(ISequenceDefinitionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteSequenceDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
