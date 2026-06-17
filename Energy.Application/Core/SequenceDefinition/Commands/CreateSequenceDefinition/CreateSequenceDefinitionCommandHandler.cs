using Energy.Application.Core.SequenceDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.SequenceDefinition.Commands.CreateSequenceDefinition;

/// <summary>
/// <see cref="CreateSequenceDefinitionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ISequenceDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateSequenceDefinitionCommandHandler
    : IRequestHandler<CreateSequenceDefinitionCommand, BaseResponse<Guid>>
{
    private readonly ISequenceDefinitionService _service;

    public CreateSequenceDefinitionCommandHandler(ISequenceDefinitionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateSequenceDefinitionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
