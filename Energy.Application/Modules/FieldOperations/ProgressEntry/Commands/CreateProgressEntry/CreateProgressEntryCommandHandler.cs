using Energy.Application.Modules.FieldOperations.ProgressEntry.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.ProgressEntry.Commands.CreateProgressEntry;

/// <summary>
/// <see cref="CreateProgressEntryCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProgressEntryService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProgressEntryCommandHandler
    : IRequestHandler<CreateProgressEntryCommand, BaseResponse<Guid>>
{
    private readonly IProgressEntryService _service;

    public CreateProgressEntryCommandHandler(IProgressEntryService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProgressEntryCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
